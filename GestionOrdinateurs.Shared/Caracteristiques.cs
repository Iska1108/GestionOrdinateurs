// TP4 : caractéristiques techniques d'un ordinateur
// Avec EF Core, c'est un type "possédé" (OwnsOne) : ses colonnes sont stockées dans la table Ordinateur
public class Caracteristiques
{
    // Mémoire vive en Go
    public int RamGo { get; set; }

    // Portable, Fixe ou Server
    public TypeOrdinateur TypeOrdi { get; set; }

    // Capacité de stockage en Go
    public int StockageGo { get; set; }

    // TP4 : marque et processeur déplacés depuis la classe Ordinateur
    public string Marque { get; set; }

    public string Processeur { get; set; }

    // EF Core et le JSON de l'API utilisent ce constructeur (les noms des paramètres correspondent aux propriétés)
    public Caracteristiques(int ramGo, int stockageGo, TypeOrdinateur typeOrdi, string marque, string processeur)
    {
        RamGo = ramGo;
        StockageGo = stockageGo;
        TypeOrdi = typeOrdi;
        Marque = marque;
        Processeur = processeur;
    }
}
