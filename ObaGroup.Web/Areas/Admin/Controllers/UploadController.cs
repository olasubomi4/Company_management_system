using Microsoft.AspNetCore.Mvc;
using ObaGroupUtility;

namespace Oba_group2.Areas.Admin.Controllers
{
    public class UploadController : Controller
    {
 
        [HttpGet]
        [Route(Constants.UploadPage)]
        public IActionResult Upload()
        {
            return File("~/dashboard/upload/index.html", "text/html");
        }
    }
}
