using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// TP4 : renommé en ReparationsController pour avoir la route api/reparations
// TP5 : il faut être connecté pour utiliser ce controleur, la simulation et la consommation de stock sont réservées aux Admin
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ReparationsController : ControllerBase
{
    private readonly ReparationService _reparService;
    private readonly StockService _stockService;
    private readonly IOrdinateurService _ordinateurService;
    private readonly ILogger<ReparationsController> _logger;

    public ReparationsController(ReparationService reparService, StockService stockService, IOrdinateurService ordinateurService, ILogger<ReparationsController> logger)
    {
        _reparService = reparService;
        _stockService = stockService;
        _ordinateurService = ordinateurService;
        _logger = logger;
    }

    // TP3 : route pour simuler 3 réparations en parallèle
    // minimum 3 ordinateurs dans la base de données pour que ça fonctionne (sinon ca 400)
    // POST api/reparations/simuler : lance 3 réparations en même temps
    // TP4 : chaque réparation est faite par un technicien
    // TP5 : réservé aux Admin
    [Authorize(Roles = "Admin")]
    [HttpPost("simuler")]
    public async Task<ActionResult> SimulerReparation()
    {
        // Récupération de la liste d'ordinateurs
        var listOrdinateurs = await _ordinateurService.GetAllAsync();

        await Task.WhenAll(
            _reparService.SimulerReparationAsync(listOrdinateurs[0], "Karim"),
            _reparService.SimulerReparationAsync(listOrdinateurs[1], "Sophie"),
            _reparService.SimulerReparationAsync(listOrdinateurs[2], "Lucas")
        );

        return Ok();
    }

    // GET api/reparations : liste des réparations avec l'ordinateur correspondant
    [HttpGet]
    public async Task<ActionResult<List<Reparation>>> GetAll()
    {
        return await _reparService.GetAllAsync();
    }

    // TP4 : GET api/reparations/historique : historique des réparations (records)
    [HttpGet("historique")]
    public async Task<ActionResult<List<ReparationService.HistoriqueReparationRecord>>> GetHistorique()
    {
        _logger.LogInformation("Récupération de l'historique des réparations");
        return await _reparService.GetHistoriqueAsync();
    }

    // GET api/reparations/stockssd : consomme 10 SSD en parallèle puis renvoie le stock restant
    // TP5 : réservé aux Admin
    [Authorize(Roles = "Admin")]
    [HttpGet("stockssd")]
    public ActionResult<int> GetStockSSD()
    {
        Parallel.For(0, 10, i => _stockService.ConsommerSSD());

        _logger.LogInformation("StockSSD : {StockSSD}", _stockService.StockSSD);
        return _stockService.StockSSD;
    }
}
