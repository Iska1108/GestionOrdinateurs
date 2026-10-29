using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// TP4 : Ordinateur contient ses réparations et une réparation contient son ordinateur,
// on ignore cette boucle lors de la conversion en JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Swagger
builder.Services.AddEndpointsApiExplorer();

// TP5 : swagger avec le bouton "Authorize" pour envoyer le token JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "GestionOrdinateurs",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Entrez uniquement le token (swagger ajoute \"Bearer\" tout seul)"
    });

    // Sans cette ligne swagger n'envoie pas le token dans les requêtes
    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

// TP5 : configuration JWT (la clé doit être la même que celle de AuthentificationApi)
var jwtKey = builder.Configuration["Jwt:Key"] ?? "";
var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            Console.WriteLine($"Utilisateur utilisant un Token Valide --- {context.Principal?.Identity?.Name ?? "Anonymous"}");
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("Token Invalide");
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

// TP5 : CORS pour autoriser le client Blazor à appeler l'API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("http://localhost:5152") // URL du client Blazor
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Contexte de la base de données (la connexion SQLite est configurée dans OnConfiguring)
builder.Services.AddDbContext<ParcInformatiqueContext>();

builder.Services.AddScoped<IOrdinateurService, OrdinateurService>();
// TP3 : ajout du service de réparation et du service de stock
// C'est du AddScoped parce que chaque requête HTTP a son propre contexte (DbContext) et donc son propre service de réparation et de stock
builder.Services.AddScoped<ReparationService>();
builder.Services.AddScoped<StockService>();
// TP4 : un seul JournalService pour toute l'application (AddSingleton), son lock protège donc bien l'écriture dans les fichiers
builder.Services.AddSingleton<JournalService>();

var app = builder.Build();

// TP5 : initialisation de la base de données au lancement de l'API
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ParcInformatiqueContext>();

    // Applique les migrations qui manquent
    db.Database.Migrate();

    // Ajout de quelques utilisateurs si la table est vide
    if (!db.Users.Any())
    {
        db.Users.AddRange(
            new User { Login = "user1", Role = RoleEnum.Admin },
            new User { Login = "user2", Role = RoleEnum.Gestionnaire },
            new User { Login = "user3", Role = RoleEnum.Gestionnaire }
        );

        db.SaveChanges();
    }
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// TP5 : CORS avant l'authentification, puis authentification (qui es-tu ?) avant autorisation (as-tu le droit ?)
app.UseCors("AllowBlazorClient");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
