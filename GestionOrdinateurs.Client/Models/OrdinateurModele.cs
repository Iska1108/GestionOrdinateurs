namespace GestionOrdinateurs.Client.Models;

//TP5 : ordinateur reçu de l'API (seulement les champs affichés dans la liste)
public class OrdinateurModele
{
    public int Id { get; set; }

    public string Nom { get; set; } = string.Empty;

    public decimal Prix { get; set; }

    public bool Vendu { get; set; }
}
