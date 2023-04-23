using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ObaGroupModel.Validation;

public class AllowedFileExtensionsAttribute : ValidationAttribute
{
    private readonly string[] extensions;

    public AllowedFileExtensionsAttribute(string[] extensions)
    {
        this.extensions = extensions;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {

        if (value == null)
        {
            return ValidationResult.Success;
        }

        var file = value as IFormFile;

        var fileExtension = Path.GetExtension(file.FileName);

        if (!extensions.Contains(fileExtension))
        {
            return new ValidationResult($"The {validationContext.DisplayName} field only allows files with the following extensions: {string.Join(", ", extensions)}");
        }

        return ValidationResult.Success;
    }
}