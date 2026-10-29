var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// TP5 : HttpClientFactory, client nommé pour appeler GestionOrdinateurs.API (adresse dans appsettings.json)
builder.Services.AddHttpClient("GestionOrdinateursApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:GestionOrdinateursApi"] ?? "http://localhost:5157/");
});

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

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowBlazorClient");

app.MapControllers();

app.Run();
