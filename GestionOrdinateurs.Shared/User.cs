using System.ComponentModel.DataAnnotations;

// TP5 : utilisateur de l'application (pas de mot de passe pour simplifier le TP)
// L'unicité du Login est configurée dans ParcInformatiqueContext (index unique)
public class User
{
    // Clé primaire
    public int Id { get; set; }

    [Required]
    public string Login { get; set; } = string.Empty;

    public RoleEnum Role { get; set; }
}
