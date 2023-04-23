using Microsoft.AspNetCore.Http;

namespace ObaGroupModel.Validation;

public class BiographyWithFile
{
    public Biography biography { get; set; }
    public List<IFormFile?> profileImage { get; set; }
    public  List<IFormFile?> profileVideo { get; set; }
}