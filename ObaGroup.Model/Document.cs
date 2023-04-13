using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ObaGroupModel;


public class Document
{
    [Key]
    public int Id { get; set; } 
    
    [Required]
    public string Name { get; set; }
    
    [ValidateNever]
    public string  Type { get; set; }
    
    [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
    public DateTime CreationDate { get; set; }
    
    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; }
    
    [Required]
    public string Staff { get; set; }
    
    [ValidateNever]
    public string DocumentUrl { get; set; }
   
}