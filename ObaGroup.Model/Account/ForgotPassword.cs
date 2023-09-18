using System.ComponentModel.DataAnnotations;

namespace ObaGroupModel;

public class ForgotPassword
{
    [Required] [EmailAddress] public string Email { get; set; }
}