var builder = WebApplication.CreateBuilder(args);

//TP5 : YARP, les routes sont lues dans la section "ReverseProxy" de appsettings.json
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorClient", policy =>
    {
        policy.WithOrigins("http://localhost:5000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("BlazorClient");

//TP5 : redirige les appels vers GestionOrdinateurs.API
app.MapReverseProxy();

app.Run();
