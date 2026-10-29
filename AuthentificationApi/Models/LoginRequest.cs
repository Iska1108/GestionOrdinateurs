using System.ComponentModel.DataAnnotations;

// TP5 : corps de la requête POST api/authentication/login
public class LoginRequest
{
    [Required]
    public string Login { get; set; } = string.Empty;
}
