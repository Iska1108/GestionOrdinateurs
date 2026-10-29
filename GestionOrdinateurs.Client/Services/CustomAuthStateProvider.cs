using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace GestionOrdinateurs.Client.Services;

// TP5 : indique à Blazor qui est connecté, à partir du token JWT gardé en session
// C'est ce qui permet à <AuthorizeView> (menu) de savoir si on affiche "Connexion" ou "Déconnexion"
public class CustomAuthStateProvider : AuthenticationStateProvider
{
    // Clé sous laquelle le token est rangé dans la session du navigateur
    public const string CleToken = "authToken";

    private readonly ISessionStorageService _sessionStorage;

    // Utilisateur anonyme (personne de connecté)
    private static readonly AuthenticationState Anonyme = new(new ClaimsPrincipal(new ClaimsIdentity()));

    public CustomAuthStateProvider(ISessionStorageService sessionStorage)
    {
        _sessionStorage = sessionStorage;
    }

    // Appelé par Blazor pour connaître l'utilisateur courant
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _sessionStorage.GetItemAsync<string>(CleToken);
        if (string.IsNullOrWhiteSpace(token))
        {
            return Anonyme;
        }

        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

        // Token expiré (15 minutes) : on le supprime et on considère l'utilisateur déconnecté
        if (jwt.ValidTo < DateTime.UtcNow)
        {
            await _sessionStorage.RemoveItemAsync(CleToken);
            return Anonyme;
        }

        // AuthentificationApi met le nom dans ClaimTypes.Name et le rôle dans ClaimTypes.Role
        var identity = new ClaimsIdentity(jwt.Claims, "jwt", ClaimTypes.Name, ClaimTypes.Role);
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    // Enregistre le token en session et prévient Blazor que l'utilisateur a changé
    public async Task ConnecterAsync(string token)
    {
        await _sessionStorage.SetItemAsync(CleToken, token);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    // Supprime le token de la session et prévient Blazor
    public async Task DeconnecterAsync()
    {
        await _sessionStorage.RemoveItemAsync(CleToken);
        NotifyAuthenticationStateChanged(Task.FromResult(Anonyme));
    }
}
