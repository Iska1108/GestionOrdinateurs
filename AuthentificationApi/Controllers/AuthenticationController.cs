using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

// TP5 : génère les tokens JWT des utilisateurs
[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<AuthenticationController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
    }

    // POST api/authentication/login : reçoit un login et renvoie un token JWT valable 15 minutes
    // 404 si l'utilisateur n'existe pas dans GestionOrdinateurs.API
    [HttpPost("login")] //petite route pour le login, on pourrait faire un GET mais c'est pas très sécurisé de mettre le login dans l'URLs
    public async Task<ActionResult<TokenResponse>> Login(LoginRequest request)
    {
        _logger.LogInformation("Tentative de connexion de {Login}", request.Login);

        // Client nommé configuré dans Program.cs (adresse de GestionOrdinateurs.API)
        var client = _httpClientFactory.CreateClient("GestionOrdinateursApi");

        HttpResponseMessage response;
        try
        {   
            // EscapeDataString pour ne pas casser l'URL si le login contient des caractères spéciaux
            response = await client.GetAsync($"api/users/login/{Uri.EscapeDataString(request.Login)}");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "GestionOrdinateurs.API injoignable");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "L'API GestionOrdinateurs est injoignable");
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return NotFound($"L'utilisateur {request.Login} n'existe pas");
        }
        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, "Erreur lors de la récupération de l'utilisateur");
        }

        var user = await response.Content.ReadFromJsonAsync<User>();
        if (user == null)
        {
            return NotFound($"L'utilisateur {request.Login} n'existe pas");
        }

        var expiration = DateTime.UtcNow.AddMinutes(15); // Le token expire dans 15 minutes c'est ce que je pense être le mieux pour la sécurité, mais on peut changer si besoin
        var token = GenererToken(user, expiration);

        return Ok(new TokenResponse
        {
            Token = token,
            Login = user.Login,
            Role = user.Role.ToString(),
            Expiration = expiration
        });
    }

    // Crée le token JWT signé avec la clé de appsettings.json (Jwt:Key)
    private string GenererToken(User user, DateTime expiration)
    {
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "");

        // Les claims sont les infos de l'utilisateur stockées dans le token
        // ClaimTypes.Role est lu par [Authorize(Roles = "Admin")] dans GestionOrdinateurs.API
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: expiration,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
