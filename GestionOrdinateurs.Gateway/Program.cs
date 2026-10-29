var builder = WebApplication.CreateBuilder(args);

//TP5 : YARP, les routes sont lues dans la section "ReverseProxy" de appsettings.json
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

//TP5 : redirige les appels vers GestionOrdinateurs.API
app.MapReverseProxy();

app.Run();
