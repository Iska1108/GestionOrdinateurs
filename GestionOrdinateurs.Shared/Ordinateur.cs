using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

// TP4 : une seule entité Ordinateur (OrdinateurFixe et OrdinateurPortable supprimés), le type (portable, fixe, serveur) est dans ses caractéristiques
// TP4 : nom de la table dans la base
[Table("Ordinateur")]
public class Ordinateur
{
    public int Id { get; set; }

    public string Nom { get; set; }

    // TP4 : l'enum EtatOrdinateur est maintenant dans son propre fichier
    public EtatOrdinateur Etat { get; set; }

    // TP4 : quantité en stock (0 par défaut au lieu de 1)
    [DefaultValue(0)]
    public int Stock { get; set; }

    // TP4 : prix en euros, passé de double à decimal (0 par défaut)
    [DefaultValue(0)]
    public decimal Prix { get; set; }

    //TP5 : indique si l'ordinateur est vendu (false par défaut)
    [DefaultValue(false)]
    public bool Vendu { get; set; } = false;

    // TP4 : marque, processeur, RAM, stockage et type de l'ordinateur
    public Caracteristiques Caracteristiques { get; set; }

    // TP4 : réparations effectuées sur cet ordinateur (relation 1 ordinateur -> plusieurs réparations)
    public List<Reparation> Reparations { get; set; } = new();
}
