using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ObaGroupModel;

public class Biographies
{
    [ValidateNever] public List<IFormFile> ProfileImage { get; set; }

    [ValidateNever] public List<IFormFile> ProfileVideo { get; set; }

    public Biography Biography { get; set; }
}