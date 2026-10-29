public interface IOrdinateurService
{
    // Évènements déclenchés lors d'un ajout, d'une suppression ou d'une mise à jour
    event EventHandler<OrdinateurAjouteEventArgs> OrdinateurAjoute;
    event EventHandler<OrdinateurSupprimeEventArgs> OrdinateurSupprime;
    event EventHandler<OrdinateurMisAJourEventArgs> OrdinateurMisAJour;

    Task<List<Ordinateur>> GetAllAsync();
    Task<Ordinateur?> GetByIdAsync(int id);
    Task<List<Ordinateur>> GetByMarqueAsync(string marque);
    // TP3 : ordinateurs disponibles (stock > 0) et triés par prix décroissant
    Task<List<Ordinateur>> GetDisponiblesAsync();
    Task<List<Ordinateur>> GetParPrixDecroissantAsync();
    Task AddAsync(Ordinateur ordinateur);
    // Ils mettent aussi a jour et suppriment les ordinateurs du parc 
    Task<bool> DeleteAsync(int id);
    Task<bool> UpdateAsync(int id, Ordinateur ordinateurModifie);
    // TP4 : diagnostic d'un ordinateur (pattern matching) et statistiques du parc (tuple nommé)
    string Diagnostiquer(Ordinateur ordinateur);
    (int TotalOrdinateur, int OrdiDisponibles, decimal ValeurStock, int NbReparations) CalculerStatistiques();
}
