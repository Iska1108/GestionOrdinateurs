// Abonnés aux évènements du service : ils affichent un message dans la console
public class OrdinateurAbonnes
{
    public static void AfficherMessageAjout(object? sender, OrdinateurAjouteEventArgs e)
    {
        Console.WriteLine($"[Évènement] Ordinateur ajouté : {e.Ordinateur.Id} - {e.Ordinateur.Nom}");
    }

    public static void AfficherMessageSuppression(object? sender, OrdinateurSupprimeEventArgs e)
    {
        Console.WriteLine($"[Évènement] Ordinateur supprimé : {e.Ordinateur.Id} - {e.Ordinateur.Nom}");
    }

    public static void AfficherMessageMiseAJour(object? sender, OrdinateurMisAJourEventArgs e)
    {
        Console.WriteLine($"[Évènement] Ordinateur mis à jour : {e.Ordinateur.Id} - {e.Ordinateur.Nom}");
    }
}
