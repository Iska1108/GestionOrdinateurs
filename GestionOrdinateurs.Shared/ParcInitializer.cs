
public class ParcInitializer
{
    public static List<Ordinateur> InitialiserParc()
    {
        var parc = new List<Ordinateur>();

        // TP4 : il n'y a plus de classes OrdinateurFixe / OrdinateurPortable, le type est dans les caractéristiques
        // Création d'ordinateurs fixes
        var ordinateurFixe1 = new Ordinateur
        {
            Id = 1,
            Nom = "Ordinateur Fixe 1",
            Etat = EtatOrdinateur.EnService,
            Stock = 3,
            Prix = 1200,
            Caracteristiques = new Caracteristiques(16, 512, TypeOrdinateur.Fixe, "Dell", "Intel Core i7")
        };

        var ordinateurFixe2 = new Ordinateur
        {
            Id = 2,
            Nom = "Ordinateur Fixe 2",
            Etat = EtatOrdinateur.EnMaintenance,
            Stock = 1,
            Prix = 800,
            Caracteristiques = new Caracteristiques(8, 256, TypeOrdinateur.Fixe, "HP", "AMD Ryzen 5")
        };

        // Création d'ordinateurs portables
        var ordinateurPortable1 = new Ordinateur
        {
            Id = 3,
            Nom = "Ordinateur Portable 1",
            Etat = EtatOrdinateur.EnService,
            Stock = 2,
            Prix = 900,
            Caracteristiques = new Caracteristiques(8, 512, TypeOrdinateur.Portable, "Lenovo", "Intel Core i5")
        };

        var ordinateurPortable2 = new Ordinateur
        {
            Id = 4,
            Nom = "Ordinateur Portable 2",
            Etat = EtatOrdinateur.HorsService,
            Stock = 0,
            Prix = 1100,
            Caracteristiques = new Caracteristiques(16, 1000, TypeOrdinateur.Portable, "Asus", "AMD Ryzen 7")
        };

        var ordinateurPortable3 = new Ordinateur
        {
            Id = 5,
            Nom = "Ordinateur Portable 3",
            Etat = EtatOrdinateur.Lent,
            Stock = 1,
            Prix = 400,
            Caracteristiques = new Caracteristiques(4, 128, TypeOrdinateur.Portable, "Acer", "Intel Core i3")
        };

        // Ajout des ordinateurs au parc
        parc.Add(ordinateurFixe1);
        parc.Add(ordinateurFixe2);
        parc.Add(ordinateurPortable1);
        parc.Add(ordinateurPortable2);
        parc.Add(ordinateurPortable3);

        return parc;
    }
}
