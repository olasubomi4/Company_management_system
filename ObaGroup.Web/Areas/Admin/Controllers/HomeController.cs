using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Oba_group2.Models;

namespace Oba_group2.Controllers;

[Area("Admin")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public HomeController(ILogger<HomeController> logger,IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
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
   // public IActionResult GetCsrfToken()
   //{
        public IActionResult SetAllCookies()
   {
       var context = _httpContextAccessor.HttpContext;
            // Get the cookie collection from the request
            var cookies = context.Request.Cookies;

            // Serialize the cookies to a JSON string
            string json = JsonSerializer.Serialize(cookies);
            Console.WriteLine(json.ToString());
            // Add the JSON string to a new cookie
            context.Response.Cookies.Append("AllCookies", json, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });
            return NoContent();
        }
        /*  var antiForgery = HttpContext.RequestServices.GetService<IAntiforgery>();
        var tokens = antiForgery.GetAndStoreTokens(HttpContext);
       
        // Take request token (which is different from a cookie token)
        var headerToken = tokens.RequestToken;
        // Set another cookie for a request token
        Response.Cookies.Append("RequestVerificationToken", headerToken, new CookieOptions
        {
            HttpOnly = false
        });
        return NoContent();
        */
}