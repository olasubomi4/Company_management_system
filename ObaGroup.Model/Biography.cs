using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ObaGroupModel;

public class Biography
{
    [Key] public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Position { get; set; }

    //   [AllowedFileExtensions(new []{".jpg", ".png", ".gif",".jpeg"})]
    [ValidateNever] public string profileImageUrl { get; set; }

    //[AllowedFileExtensions(new []{".mp4", ".mkv",".mov",".avi"})]
    [ValidateNever] public string profileVideoUrl { get; set; }
}