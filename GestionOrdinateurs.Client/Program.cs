using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GestionOrdinateurs.Client;
using GestionOrdinateurs.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// TP5 : client nommé pour appeler AuthentificationApi (adresse dans wwwroot/appsettings.json)
builder.Services.AddHttpClient("AuthentificationApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Apis:AuthentificationApi"] ?? "http://localhost:5118/");
});

//TP5 : client nommé pour passer par la Gateway (les URLs directes des APIs ne sont plus utilisées)
builder.Services.AddHttpClient("Gateway", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Apis:Gateway"]!);
});

// TP5 : stockage en session du navigateur (le token y est gardé)
builder.Services.AddBlazoredSessionStorage();

// TP5 : authentification côté Blazor, basée sur le token en session
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthStateProvider>());

await builder.Build().RunAsync();
