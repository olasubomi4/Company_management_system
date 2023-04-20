using System.ComponentModel.DataAnnotations;

namespace ObaGroupModel;

public class EmailVerification
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}