using Microsoft.AspNetCore.Mvc;

namespace Oba_group2.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return File("~/login/index.html", "text/html");

        }
    }
}
