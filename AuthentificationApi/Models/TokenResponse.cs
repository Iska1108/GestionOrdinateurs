// TP5 : réponse renvoyée au client après une connexion réussie
public class TokenResponse
{
    public string Token { get; set; } = string.Empty;

    public string Login { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    // Date (UTC) à laquelle le token n'est plus valide
    public DateTime Expiration { get; set; }
}
