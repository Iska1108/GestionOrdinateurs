// Réparation effectuée sur un ordinateur de l'atelier

public class Reparation
{
    public int Id { get; set; }

    public int OrdinateurId { get; set; }

    public Ordinateur UnOrdinateur { get; set; }

    public string TypeReparation { get; set; }

    public DateTime DateDebut { get; set; }

    public string Statut { get; set; }

    // TP4 : nom du technicien qui a fait la réparation
    public string Technicien { get; set; }
}
