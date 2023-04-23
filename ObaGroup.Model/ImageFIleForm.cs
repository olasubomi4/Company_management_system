using Microsoft.AspNetCore.Http;
using ObaGroupModel.Validation;

namespace ObaGroupModel;

public class ImageFIleForm
{
    [AllowedFileExtensions(new []{".jpg", ".png", ".gif"})]
    public IFormFile? Image { get; set; }
}