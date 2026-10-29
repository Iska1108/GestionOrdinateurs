// TP4 : service qui écrit les actions de l'atelier dans des fichiers de logs
public class JournalService
{
    private readonly string _fichierReparations = Path.Combine("Logs", "Reparations", "reparations.log");
    private readonly string _fichierOrdinateurs = Path.Combine("Logs", "Ordinateurs", "ordinateurs.log");

    // Les réparations sont simulées en parallèle : le lock évite que deux threads écrivent dans le même fichier en même temps
    private readonly object _verrou = new object();

    public JournalService()
    {
        // On crée les dossiers s'ils n'existent pas encore (ne fait rien s'ils existent déjà)
        Directory.CreateDirectory(Path.Combine("Logs", "Reparations"));
        Directory.CreateDirectory(Path.Combine("Logs", "Ordinateurs"));
    }

    // Écrit une ligne dans le log des réparations
    public void EcrireReparation(string message)
    {
        Ecrire(_fichierReparations, message);
    }

    // Écrit une ligne dans le log des ordinateurs (ajout, modification, suppression)
    public void EcrireOrdinateur(string message)
    {
        Ecrire(_fichierOrdinateurs, message);
    }

    // Ajoute une ligne "date du jour - message" à la fin du fichier
    // using : le StreamWriter est fermé (Dispose) automatiquement à la fin de la méthode, même en cas d'erreur
    private void Ecrire(string fichier, string message)
    {
        lock (_verrou)
        {
            using var writer = new StreamWriter(fichier, append: true);

            writer.WriteLine($"{DateTime.Now:dd/MM/yyyy} - {message}");
        }
    }
}
