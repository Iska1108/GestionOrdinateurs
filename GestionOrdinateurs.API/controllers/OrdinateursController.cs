using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// TP5 : il faut être connecté (token JWT valide) pour utiliser ce controleur
// Les lectures sont ouvertes à tous les rôles, les modifications sont réservées aux Admin
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrdinateursController : ControllerBase
{
    private readonly IOrdinateurService _ordinateurService;
    private readonly ILogger<OrdinateursController> _logger;
    private readonly IConfiguration _configuration;

    // Le service, le logger et la configuration sont injectés automatiquement
    public OrdinateursController(IOrdinateurService ordinateurService, ILogger<OrdinateursController> logger, IConfiguration configuration)
    {
        _ordinateurService = ordinateurService;
        _logger = logger;
        _configuration = configuration;

        // Abonnement aux évènements du service
        _ordinateurService.OrdinateurAjoute += OrdinateurAbonnes.AfficherMessageAjout;
        _ordinateurService.OrdinateurSupprime += OrdinateurAbonnes.AfficherMessageSuppression;
        _ordinateurService.OrdinateurMisAJour += OrdinateurAbonnes.AfficherMessageMiseAJour;
    }

    // GET api/ordinateurs : liste de tout le parc
    [HttpGet]
    public async Task<ActionResult<List<Ordinateur>>> GetAll()
    {
        _logger.LogInformation("Récupération de tous les ordinateurs");
        return await _ordinateurService.GetAllAsync();
    }

    // TP3 : ajout de deux routes pour les ordinateurs disponibles et triés par prix décroissant 
    // J'ai mis les logs juste pour voir que ça marche, mais on peut les enlever si vous voulez (dans un projet réel, on ne met pas de log pour chaque requête, ça pollue le fichier de log nan ?)
    
    // GET api/ordinateurs/disponibles : ordinateurs ayant un stock > 0
    [HttpGet("disponibles")]
    public async Task<ActionResult<List<Ordinateur>>> GetDisponibles()
    {
        _logger.LogInformation("Récupération des ordinateurs disponibles");
        return await _ordinateurService.GetDisponiblesAsync();
    }

    // GET api/ordinateurs/prix : ordinateurs du plus cher au moins cher
    [HttpGet("prix")]
    public async Task<ActionResult<List<Ordinateur>>> GetParPrixDecroissant()
    {
        _logger.LogInformation("Récupération des ordinateurs triés par prix décroissant");
        return await _ordinateurService.GetParPrixDecroissantAsync();
    }

    // GET api/ordinateurs/info : infos de l'application lues dans appsettings.json
    [HttpGet("info")]
    public ActionResult GetInfo()
    {
        _logger.LogInformation("Récupération des informations de l'application");

        var nom = _configuration["Application:Name"];
        var version = _configuration["Application:Version"];

        return Ok(new { Nom = nom, Version = version });
    }

    // TP4 : GET api/ordinateurs/statistiques : statistiques du parc
    [HttpGet("statistiques")]
    public ActionResult GetStatistiques()
    {
        _logger.LogInformation("Calcul des statistiques du parc");

        // Déconstruction du tuple nommé renvoyé par le service
        var (total, enStock, valeurStock, nbReparations)
            = _ordinateurService.CalculerStatistiques();

        return Ok(new
        {
            TotalOrdinateur = total,
            OrdiDisponibles = enStock,
            ValeurStock = valeurStock,
            NbReparations = nbReparations
        });
    }

    // TP4 : GET api/ordinateurs/inventaire/{nom} : recherche avec l'indexeur par nom
    [HttpGet("inventaire/{nom}")]
    public async Task<ActionResult<Ordinateur>> Chercher(string nom)
    {
        _logger.LogInformation("Recherche de l'ordinateur {Nom} dans l'inventaire", nom);

        var inventaire = new InventaireOrdinateurs();
        inventaire.Ordinateurs = await _ordinateurService.GetAllAsync();

        var ordinateur = inventaire[nom];
        if (ordinateur == null)
        {
            return NotFound();
        }
        return ordinateur;
    }

    // TP4 : GET api/ordinateurs/{id}/diagnostic : action conseillée pour un ordinateur
    [HttpGet("{id}/diagnostic")]
    public async Task<ActionResult> GetDiagnostic(int id)
    {
        _logger.LogInformation("Diagnostic de l'ordinateur {Id}", id);

        var ordinateur = await _ordinateurService.GetByIdAsync(id);
        if (ordinateur == null)
        {
            return NotFound();
        }

        var diagnostic = _ordinateurService.Diagnostiquer(ordinateur);
        return Ok(new { ordinateur.Id, ordinateur.Nom, Diagnostic = diagnostic });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Ordinateur>> GetById(int id)
    {
        _logger.LogInformation("Récupération de l'ordinateur {Id}", id);

        var ordinateur = await _ordinateurService.GetByIdAsync(id);
        if (ordinateur == null)
        {
            return NotFound();
        }
        return ordinateur;
    }

    // Recherche des ordinateurs par marque
    [HttpGet("marque/{marque}")]
    public async Task<ActionResult<List<Ordinateur>>> GetByMarque(string marque)
    {
        _logger.LogInformation("Recherche des ordinateurs de la marque {Marque}", marque);
        return await _ordinateurService.GetByMarqueAsync(marque);
    }

    // Renvoie un 201 avec l'adresse du nouvel ordinateur
    // TP5 : réservé aux Admin
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> Add(Ordinateur ordinateur)
    {
        _logger.LogInformation("Ajout de l'ordinateur {Nom}", ordinateur.Nom);

        await _ordinateurService.AddAsync(ordinateur);
        return CreatedAtAction(nameof(GetById), new { id = ordinateur.Id }, ordinateur);
    }

    // TP5 : réservé aux Admin
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        _logger.LogInformation("Suppression de l'ordinateur {Id}", id);

        var success = await _ordinateurService.DeleteAsync(id);
        if (!success)
        {
            return NotFound();
        }
        return Ok();
    }

    // TP5 : réservé aux Admin
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, Ordinateur ordinateurModifie)
    {
        _logger.LogInformation("Mise à jour de l'ordinateur {Id}", id);

        var success = await _ordinateurService.UpdateAsync(id, ordinateurModifie);
        if (!success)
        {
            return NotFound();
        }
        return Ok();
    }
}
