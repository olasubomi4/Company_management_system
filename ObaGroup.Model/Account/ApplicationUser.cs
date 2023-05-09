using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ObaGroupModel;

public class ApplicationUser : IdentityUser
{
    [Required]
    [Display(Name = "First name")]
    public string? FirstName { get; set; }
    
    [Required]
    [Display(Name = "Last name")]
    
    public string? LastName { get; set; }
    
    [Required]
    [Display(Name = "Position")]
    public string? Position { get; set; }
    
    [ValidateNever]
    [Display(Name = "Address")]
    public string? Address { get; set; }
    
    public string? ImageUrl { get; set; }
    
}