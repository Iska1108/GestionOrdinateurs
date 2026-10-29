// TP4 : collection spécialisée d'ordinateurs avec deux indexeurs
// Permet d'écrire inventaire["Dell XPS"] ou inventaire[3] au lieu d'appeler une méthode de recherche
public class InventaireOrdinateurs
{
    public List<Ordinateur> Ordinateurs { get; set; } = new();

    // Indexeur par nom : renvoie null si aucun ordinateur ne porte ce nom (sans tenir compte des majuscules/minuscules)
    public Ordinateur? this[string nom]
    {
        get { return Ordinateurs.FirstOrDefault(o => o.Nom.ToLower() == nom.ToLower()); }
    }

    // Indexeur par identifiant : renvoie null si aucun ordinateur n'a cet id
    public Ordinateur? this[int ordinateurId]
    {
        get { return Ordinateurs.FirstOrDefault(o => o.Id == ordinateurId); }
    }
}
