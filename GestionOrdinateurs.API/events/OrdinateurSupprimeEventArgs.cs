// Informations envoyées aux abonnés quand un ordinateur est supprimé
public class OrdinateurSupprimeEventArgs : EventArgs
{
    public Ordinateur Ordinateur { get; set; }

    public OrdinateurSupprimeEventArgs(Ordinateur ordinateur)
    {
        Ordinateur = ordinateur;
    }
}
