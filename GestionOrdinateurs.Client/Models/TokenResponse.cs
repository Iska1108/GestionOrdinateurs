namespace GestionOrdinateurs.Client.Models;

// TP5 : réponse de AuthentificationApi (POST api/authentication/login)
public class TokenResponse
{
    public string Token { get; set; } = string.Empty;

    public string Login { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public DateTime Expiration { get; set; }
}
