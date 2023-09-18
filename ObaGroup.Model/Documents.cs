using Microsoft.AspNetCore.Http;

namespace ObaGroupModel;

public class Documents
{
    public Document Document { get; set; }
    public List<IFormFile> Files { get; set; }
}