using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ObaGroupModel;

public class UserOauthRefreshToken
{

    [Key]
    public int Id { get; set; } 
    
    [ForeignKey("ApplicationUserId")]
    [ValidateNever]
    public ApplicationUser ApplicationUser { get; set; }
    
    [ValidateNever]
    public string RefreshToken { get; set; }
}