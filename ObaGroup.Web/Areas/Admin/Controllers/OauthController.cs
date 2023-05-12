using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ObaGoupDataAccess;
using ObaGoupDataAccess.Repository.IRepository;
using ObaGroupModel;
using ObaGroupUtility;
using RestSharp;


namespace Oba_group2.Areas.Admin.Controllers;

public  class OauthController : Controller
{
    private readonly string currentDirectory = Directory.GetCurrentDirectory();
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;

    public OauthController(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
    {
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = unitOfWork;
    }
    
    [Route(Constants.Google_Calendar_Callback_Endpoint)]
    public IActionResult callback(string code, string? error, string? state)
    {
        ResponseModel responseModel = new ResponseModel();
        if (string.IsNullOrWhiteSpace(error))
        {
            string uri= this.GetTokens(code);
           return Redirect(uri);
        }
        return Redirect(failedAttempt(error));
    }

    public string failedAttempt(string error)
    {
        return $"{Request.Scheme}://{Request.Host}{Constants.Create_Event_Endpoint}?error="+error;
    }
    
    [HttpPost]
    public string GetTokens(string code)
    {
        OAuthTokenProperties oAuthTokenProperties = new OAuthTokenProperties(_httpContextAccessor,_unitOfWork);
        var tokenFile = currentDirectory+"/tokens.json";
        var credentialsFile =
            currentDirectory+"/client_secret_87857337556-iqm8t560cfhc8ddln4mdk88ahl311na9.apps.googleusercontent.com.json";
        var credentials = JObject.Parse((System.IO.File.ReadAllText(credentialsFile)));

        RestClient restClient = new RestClient();
        RestSharp.RestRequest request = new RestSharp.RestRequest();
        
        var client_id=credentials["web"]["client_id"].ToString();
        var clientSecret = credentials["web"]["client_secret"].ToString();

        request.AddQueryParameter("client_id", client_id);
        request.AddQueryParameter("client_secret", clientSecret);
        request.AddQueryParameter("code", code);
        request.AddQueryParameter("grant_type", "authorization_code");
        request.AddQueryParameter("redirect_uri",  $"{Request.Scheme}://{Request.Host}{Constants.Google_Calendar_Callback_Endpoint}");

        restClient.BaseUrl = new System.Uri(Constants.Google_Get_Token_Endpoint);
        var response =  restClient.Post(request);
        string responseContent = response.Content.ToString();
    
        
        var token = JObject.Parse(responseContent);
        OauthToken oauthToken = new OauthToken();
        if (token != null)
        {
            if (token["access_token"] != null)
            {
                oauthToken.access_token = token["access_token"].ToString();
            }
            if (token["expires_in"] != null)
            {
                oauthToken.expires_in =Int32.Parse(token["expires_in"].ToString());
            }
            if (token["scope"] != null)
            {
                oauthToken.scope = token["scope"].ToString();
            }
            if (token["token_type"] != null)
            {
                oauthToken.token_type = token["token_type"].ToString();
            }
            if (token["id_token"] != null)
            {
                oauthToken.id_token = token["id_token"].ToString();
            }
            if (token["refresh_token"] != null)
            {
                oauthToken.refresh_token = token["refresh_token"].ToString();
            }
        }
        Console.WriteLine(oauthToken);
        
        SetCsrfToken(oauthToken);
        
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            Console.WriteLine("GETTING VALUE FROM METHOD");
            Console.WriteLine(oAuthTokenProperties.GetAccessToken());
                
            System.IO.File.WriteAllText(tokenFile,response.Content);
            Console.WriteLine("hello");
            Console.WriteLine(response.Content);
            return $"{Request.Scheme}://{Request.Host}{Constants.Create_Event_Endpoint}";
        }
        return failedAttempt(response.Content);
    }
    
    
    [HttpPost]
    public IActionResult RevokeTokens()
    {
        OAuthTokenProperties oAuthTokenProperties = new OAuthTokenProperties(_httpContextAccessor,_unitOfWork);
        //var tokenFile = currentDirectory+"/tokens.json";
        //var credentialsFile = currentDirectory+"/client_secret_87857337556-iqm8t560cfhc8ddln4mdk88ahl311na9.apps.googleusercontent.com.json";
      //  var tokens = JObject.Parse((System.IO.File.ReadAllText(tokenFile)));

     
        var access_token =oAuthTokenProperties.GetAccessToken();
        Console.WriteLine(access_token);
        RestClient restClient = new RestClient();
        RestSharp.RestRequest request = new RestSharp.RestRequest();
        
        request.AddQueryParameter("token", access_token);

        restClient.BaseUrl = new System.Uri(Constants.Google_Revoke_Token_Endpoint);
        var response = restClient.Post(request);
        
        Console.WriteLine(response.Content);
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            oAuthTokenProperties.RevokeToken(access_token);
            return Ok(response.Content);
        }
        return BadRequest(response.Content);
    }
    
    public void SetCsrfToken(OauthToken oauthToken)
    {
        var context = _httpContextAccessor.HttpContext;
        
        if (oauthToken.access_token != null)
        {
            Response.Cookies.Append("OauthTokenAccessToken", oauthToken.access_token, new CookieOptions
            {
                HttpOnly = false
            });
            context.Session.SetString("OauthTokenAccessToken", oauthToken.access_token);
        }

        if (oauthToken.id_token != null)
        {
            Response.Cookies.Append("OauthTokenIdToken", oauthToken.id_token, new CookieOptions
            {
                HttpOnly = false
            });
            context.Session.SetString("OauthTokenIdToken", oauthToken.id_token);
        }

        if (oauthToken.token_type != null)
        {
            Response.Cookies.Append("OauthTokenType", oauthToken.token_type, new CookieOptions
            {
                HttpOnly = false
            });
            context.Session.SetString("OauthTokenType", oauthToken.token_type);
        }

        if (oauthToken.refresh_token != null)
        {
            UserOauthRefreshToken userOauthRefreshToken = new UserOauthRefreshToken();
            var user = _httpContextAccessor.HttpContext.User;
            var userEmail = user.FindFirst(ClaimTypes.Email)?.Value;
            ApplicationUser applicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Email == userEmail);

            userOauthRefreshToken.RefreshToken = oauthToken.refresh_token;
            userOauthRefreshToken.ApplicationUser = applicationUser;
            _unitOfWork.UserOauthRefreshTokenRepository.Upsert(userOauthRefreshToken);
        }

        if (oauthToken.expires_in != null)
        {
            Response.Cookies.Append("OauthExpiresIn", oauthToken.expires_in.ToString(), new CookieOptions
            {
                HttpOnly = false
            });
            context.Session.SetString("OauthExpiresIn", oauthToken.expires_in.ToString());
        }

        if (oauthToken.scope != null)
        {

            Response.Cookies.Append("oauthTokenScope", oauthToken.scope, new CookieOptions
            {
                HttpOnly = false
            });
            context.Session.SetString("oauthTokenScope", oauthToken.scope);
        }
    }
}