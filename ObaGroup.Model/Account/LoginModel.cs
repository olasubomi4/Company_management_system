using System.ComponentModel.DataAnnotations;

namespace ObaGroupModel;

public class LoginModel
{
    [Required]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail.com$",
        ErrorMessage = "The Email must be a gmail account")]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Remember me?")] public bool RememberMe { get; set; }
}