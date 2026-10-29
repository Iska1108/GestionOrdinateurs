// Informations envoyées aux abonnés quand un ordinateur est mis à jour
public class OrdinateurMisAJourEventArgs : EventArgs
{
    public Ordinateur Ordinateur { get; set; }

    public OrdinateurMisAJourEventArgs(Ordinateur ordinateur)
    {
        Ordinateur = ordinateur;
    }
}
