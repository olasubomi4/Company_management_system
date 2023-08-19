using Microsoft.AspNetCore.Mvc;
using ObaGroupUtility;

namespace Oba_group2.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
            
        private static readonly ILogger _logger = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        }).CreateLogger("Home");
        [HttpGet("")]
        public IActionResult Index()
        {
            
            _logger.LogCritical("redirect to login");
            var redirectUrl = $"{Request.Scheme}://{Request.Host}{Constants.Login_Endpoint}";
            return Redirect(redirectUrl);

        }
    }
}
