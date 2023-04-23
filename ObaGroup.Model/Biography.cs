using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ObaGroupModel.Validation;

namespace ObaGroupModel;

public class Biography
{
    [Key] 
    public int id { get; set; }
    [Required]
    public string firstName { get; set; }
    [Required]
    public string lastName { get; set; }
    [Required]
    public string position { get; set; }
    
    [AllowedFileExtensions(new []{".jpg", ".png", ".gif"})]
    public string? profileImageUrl { get; set; }
 
    [AllowedFileExtensions(new []{".mp4", ".mkv"})]
    public string? profileVideoUrl { get; set; }

}

    