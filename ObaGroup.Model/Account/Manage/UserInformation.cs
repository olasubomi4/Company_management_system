using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ObaGroupModel;

public class UserInformation
{
    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [TempData]
    public string StatusMessage { get; set; }

    [Phone]
    [MaxLength(11, ErrorMessage = "Phone number lenght cannot be more than 11")]
    [Display(Name = "Phone number")]
    public string PhoneNumber { get; set; }

    [Display(Name = "First name")] public string FirstName { get; set; }

    [Display(Name = "Last name")] public string LastName { get; set; }

    [Display(Name = "Address")] public string Address { get; set; }

    [Display(Name = "Position")] public string Position { get; set; }
}