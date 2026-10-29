namespace GestionOrdinateurs.Client.Models;

//TP5 : ligne de l'historique des réparations reçue de l'API (api/reparations/historique)
public class HistoriqueReparationModele
{
    public int OrdinateurId { get; set; }

    public string NomOrdinateur { get; set; } = string.Empty;

    public string Technicien { get; set; } = string.Empty;

    public string TypeReparation { get; set; } = string.Empty;

    public DateTime DateIntervention { get; set; }
}
