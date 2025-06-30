using System.ComponentModel.DataAnnotations;

namespace BlazorSchulung2025.Models;

public class UserInfo
{
    [Display(Name = "Benutzername")]
    [MaxLength(4, ErrorMessage = "Username zu lang (max. 4 Zeichen)")]
    public string UserName { get; set; } = string.Empty;
    
    [MaxLength(40, ErrorMessage = "E-Mailadresse zu lang (max. 40 Zeichen)")]
    [EmailAddress(ErrorMessage = "Das ist keine E-Mailadresse")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Vorname ist erforderlich")]
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    
    
    public DateTime Birthday { get; set; } = DateTime.Now.AddYears(-20);
}