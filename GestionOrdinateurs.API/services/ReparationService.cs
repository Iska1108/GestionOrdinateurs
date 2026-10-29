using Microsoft.EntityFrameworkCore;

// Service qui gère les réparations de l'atelier
public class ReparationService
{
    // TP4 : record qui représente une ligne de l'historique des réparations (données immuables renvoyées par l'API)
    public record HistoriqueReparationRecord(
        int OrdinateurId,
        string NomOrdinateur,
        string Technicien,
        string TypeReparation,
        DateTime DateIntervention
    );

    private readonly ParcInformatiqueContext _context;
    private readonly IServiceScopeFactory _scopeFactory;
    // TP4 : journal pour écrire les réparations dans les logs
    private readonly JournalService _journalService;

    // TP4 : ajout du JournalService dans le constructeur
    public ReparationService(ParcInformatiqueContext context, IServiceScopeFactory scopeFactory, JournalService journalService)
    {
        _context = context;
        _scopeFactory = scopeFactory;
        _journalService = journalService;
    }

    // TP3 : Méthode pour réparer un ordinateur (simule une réparation de 2 secondes)
    // Méthode privée pour créer une réparation dans la base de données
    // TP4 : ajout du paramètre technicien
    private async Task<Reparation> CreerReparationAsync(ParcInformatiqueContext context, Ordinateur ordinateur, string technicien)
    {
        var reparation = new Reparation
        {
            OrdinateurId = ordinateur.Id,
            TypeReparation = "Diagnostic",
            DateDebut = DateTime.Now,
            Statut = "En cours",
            Technicien = technicien
        };

        context.Reparations.Add(reparation);
        await context.SaveChangesAsync();
        return reparation;
    }

    // Un DbContext ne supporte pas plusieurs opérations en même temps,
    // donc chaque simulation (lancée en parallèle) utilise son propre contexte parce que un meme dbcontexte ne âsupporte pas plusieurs opérations en même temps
    // TP4 : on précise le technicien qui fait la réparation
    public async Task SimulerReparationAsync(Ordinateur ordinateur, string technicien)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ParcInformatiqueContext>();

        var reparation = await CreerReparationAsync(context, ordinateur, technicien);

        await Task.Delay(2000);

        reparation.Statut = "Terminée";
        await context.SaveChangesAsync();

        Console.WriteLine($"{ordinateur.Nom} réparé");

        // TP4 : on garde une trace de la réparation dans le fichier de logs
        _journalService.EcrireReparation($"{ordinateur.Nom} réparé");
    }

    // Liste des réparations avec les infos de l'ordinateur (Include = jointure)
    public async Task<List<Reparation>> GetAllAsync()
    {
        return await _context.Reparations
            .Include(r => r.UnOrdinateur)
            .ToListAsync();
    }

    // TP4 : historique des réparations (de la plus ancienne à la plus récente) sous forme de records
    public async Task<List<HistoriqueReparationRecord>> GetHistoriqueAsync()
    {
        return await _context.Reparations
            .OrderBy(r => r.DateDebut)
            .Select(r => new HistoriqueReparationRecord(
                r.OrdinateurId,
                r.UnOrdinateur.Nom,
                r.Technicien,
                r.TypeReparation,
                r.DateDebut))
            .ToListAsync();
    }
}
