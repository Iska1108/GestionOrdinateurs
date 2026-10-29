using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// TP5 : controleur des utilisateurs
// Pas de [Authorize] ici : l'API d'authentification appelle "login/{login}" AVANT d'avoir un token
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ParcInformatiqueContext _context;
    private readonly ILogger<UsersController> _logger;

    public UsersController(ParcInformatiqueContext context, ILogger<UsersController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET api/users : récupère tous les utilisateurs
    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAll()
    {
        _logger.LogInformation("Récupération de tous les utilisateurs");
        return await _context.Users.ToListAsync();
    }

    // GET api/users/login/{login} : cherche un utilisateur par son login (404 s'il n'existe pas)
    [HttpGet("login/{login}")]
    public async Task<ActionResult<User>> GetByLogin(string login)
    {
        _logger.LogInformation("Recherche de l'utilisateur {Login}", login);

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
        if (user == null)
        {
            return NotFound();
        }
        return user;
    }
}
