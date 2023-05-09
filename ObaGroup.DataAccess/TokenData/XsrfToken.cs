
/*using Microsoft.AspNetCore.Antiforgery;


namespace ObaGroupUtility;

public  class XsrfToken
{
    
    public string GetCsrfToken()
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
*/