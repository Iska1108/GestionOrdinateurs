// Informations envoyées aux abonnés quand un ordinateur est ajouté
public class OrdinateurAjouteEventArgs : EventArgs
{
    public Ordinateur Ordinateur { get; set; }

    public OrdinateurAjouteEventArgs(Ordinateur ordinateur)
    {
        Ordinateur = ordinateur;
    }
}
