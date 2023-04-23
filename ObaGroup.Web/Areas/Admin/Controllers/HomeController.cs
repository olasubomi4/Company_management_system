using System.Diagnostics;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Oba_group2.Models;

namespace Oba_group2.Controllers;

[Area("Admin")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    [HttpGet]
    [Route("Admin/Dashboard/Calendar/GetCsrfToken")]
    public IActionResult GetCsrfToken()
    {
        var antiForgery = HttpContext.RequestServices.GetService<IAntiforgery>();
        var tokens = antiForgery.GetAndStoreTokens(HttpContext);
       
        // Take request token (which is different from a cookie token)
        var headerToken = tokens.RequestToken;
        // Set another cookie for a request token
        Response.Cookies.Append("XSRF-TOKEN", headerToken, new CookieOptions
        {
            HttpOnly = false
        });
        return NoContent();
    }
}