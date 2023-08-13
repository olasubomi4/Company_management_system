using Microsoft.AspNetCore.Mvc;
using ObaGroupUtility;

namespace Oba_group2.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {

            var redirectUrl = $"{Request.Scheme}://{Request.Host}{Constants.Login_Endpoint}";
            return Redirect(redirectUrl);

        }
    }
}
