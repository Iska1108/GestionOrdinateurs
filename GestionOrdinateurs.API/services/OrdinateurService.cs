using Microsoft.EntityFrameworkCore;

public class OrdinateurService : IOrdinateurService
{
    private readonly ParcInformatiqueContext _context;
    // TP4 : journal pour écrire les actions sur les ordinateurs dans les logs
    private readonly JournalService _journalService;

    public event EventHandler<OrdinateurAjouteEventArgs> OrdinateurAjoute;
    public event EventHandler<OrdinateurSupprimeEventArgs> OrdinateurSupprime;
    public event EventHandler<OrdinateurMisAJourEventArgs> OrdinateurMisAJour;

    // TP4 : ajout du JournalService dans le constructeur
    // Le contexte et le journal sont injectés automatiquement (enregistrés dans Program.cs) (Je suis désolé monsieur si jamais c'est mal rédiger)
    public OrdinateurService(ParcInformatiqueContext context, JournalService journalService)
    {
        _context = context;
        _journalService = journalService;
    }

    public async Task<List<Ordinateur>> GetAllAsync()
    {
        return await _context.Ordinateurs.ToListAsync();
    }

    // Renvoie null si aucun ordinateur n'a cette id
    public async Task<Ordinateur?> GetByIdAsync(int id)
    {
        return await _context.Ordinateurs.FirstOrDefaultAsync(o => o.Id == id);
    }

    // Recherche des ordinateurs par marque (sans tenir compte des majuscules/minuscules)
    // TP4 : la marque est maintenant dans les caractéristiques
    public async Task<List<Ordinateur>> GetByMarqueAsync(string marque)
    {
        return await _context.Ordinateurs
            .Where(o => o.Caracteristiques.Marque.ToLower() == marque.ToLower())
            .ToListAsync();
    }

    // Ordinateurs disponibles = ceux qui ont encore du stock
    public async Task<List<Ordinateur>> GetDisponiblesAsync()
    {
        return await _context.Ordinateurs
            .Where(o => o.Stock > 0)
            .ToListAsync();
    }

    // Ordinateurs triés du plus cher au moins cher
    // TP4 : le prix est un decimal, SQLite ne sait pas trier les decimal donc on trie la liste une fois chargée
    public async Task<List<Ordinateur>> GetParPrixDecroissantAsync()
    {
        var ordinateurs = await _context.Ordinateurs.ToListAsync();

        return ordinateurs
            .OrderByDescending(o => o.Prix)
            .ToList();
    }

    public async Task AddAsync(Ordinateur ordinateur)
    {
        _context.Ordinateurs.Add(ordinateur);
        await _context.SaveChangesAsync();

        // TP4 : écriture de l'ajout dans les logs
        _journalService.EcrireOrdinateur($"{ordinateur.Nom} ajouté");

        // On prévient les abonnés (le ?. évite une erreur si personne n'est abonné)
        OrdinateurAjoute?.Invoke(this, new OrdinateurAjouteEventArgs(ordinateur));
    }

    // Retourne false si l'ordinateur n'existe pas, pour que le controller puisse renvoyer un 404
    public async Task<bool> DeleteAsync(int id)
    {
        var ordinateur = await GetByIdAsync(id);

        if (ordinateur == null)
        {
            return false;
        }

        _context.Ordinateurs.Remove(ordinateur);
        await _context.SaveChangesAsync();

        // TP4 : écriture de la suppression dans les logs
        _journalService.EcrireOrdinateur($"{ordinateur.Nom} supprimé");

        OrdinateurSupprime?.Invoke(this, new OrdinateurSupprimeEventArgs(ordinateur));
        return true;
    }

    public async Task<bool> UpdateAsync(int id, Ordinateur ordinateurModifie)
    {
        var ordinateur = await GetByIdAsync(id);

        if (ordinateur == null)
        {
            return false;
        }

        // TP4 : Marque, Processeur et Ram sont maintenant dans les caractéristiques
        ordinateur.Nom = ordinateurModifie.Nom;
        ordinateur.Etat = ordinateurModifie.Etat;
        ordinateur.Stock = ordinateurModifie.Stock;
        ordinateur.Prix = ordinateurModifie.Prix;
        ordinateur.Caracteristiques = ordinateurModifie.Caracteristiques;

        await _context.SaveChangesAsync();

        // TP4 : écriture de la modification dans les logs
        _journalService.EcrireOrdinateur($"{ordinateur.Nom} modifié");

        OrdinateurMisAJour?.Invoke(this, new OrdinateurMisAJourEventArgs(ordinateur));
        return true;
    }

    // TP4 : diagnostic avec le pattern matching (pattern de propriété et pattern relationnel)
    public string Diagnostiquer(Ordinateur ordinateur)
    {
        return ordinateur switch
        {
            // Ordinateur hors service : on le remplace
            {
                Etat: EtatOrdinateur.HorsService
            }
                => "Remplacement",

            // Ordinateur lent avec moins de 8 Go de RAM : on ajoute de la RAM
            {
                Etat: EtatOrdinateur.Lent,
                Caracteristiques:
                {
                    RamGo: < 8
                }
            }
                => "Ajouter RAM",

            // Tous les autres cas
            _
                => "Aucune action"
        };
    }

    // TP4 : statistiques du parc renvoyées dans un tuple nommé
    public (int TotalOrdinateur, int OrdiDisponibles, decimal ValeurStock, int NbReparations)
        CalculerStatistiques()
    {
        // On charge la liste car SQLite ne sait pas faire la somme de decimal
        var ordinateurs = _context.Ordinateurs.ToList();

        int totalOrdinateur = ordinateurs.Count;
        int ordiDisponibles = ordinateurs.Count(o => o.Stock > 0);
        decimal valeurStock = ordinateurs.Sum(o => o.Prix * o.Stock);
        int nbReparations = _context.Reparations.Count();

        return (totalOrdinateur, ordiDisponibles, valeurStock, nbReparations);
    }
}
