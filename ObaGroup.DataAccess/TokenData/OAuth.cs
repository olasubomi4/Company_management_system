using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using ObaGoupDataAccess.Repository.IRepository;
using RestSharp;

namespace ObaGoupDataAccess;

public  class OAuth
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    private readonly OAuthTokenProperties oAuthTokenProperties;

    public OAuth(IUnitOfWork unitOfWork,IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
        oAuthTokenProperties = new OAuthTokenProperties(_httpContextAccessor, _unitOfWork);
    }
    
    private static readonly ILogger _logger = LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
    }).CreateLogger("OAuth");
    
    public Boolean RefreshTokens()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var tokenFile = currentDirectory+"/tokens.json";
        var credentialsFile =
            currentDirectory+"/client_secret_87857337556-iqm8t560cfhc8ddln4mdk88ahl311na9.apps.googleusercontent.com.json";
        var credentials = JObject.Parse((System.IO.File.ReadAllText(credentialsFile)));
       // var tokens = JObject.Parse((System.IO.File.ReadAllText(tokenFile)));

        RestClient restClient = new RestClient();
        RestSharp.RestRequest request = new RestSharp.RestRequest();
        
        var client_id=credentials["web"]["client_id"].ToString();
        var clientSecret = credentials["web"]["client_secret"].ToString();

        //var idToken = oAuthTokenProperties.GetIdToken();
       // var handler = new JwtSecurityTokenHandler();
        //var token = handler.ReadJwtToken(idToken);
        
        //Console.WriteLine(token.ToString());
        //Console.WriteLine(email);
        
        _logger.LogInformation("checking if refresh token exist");

        var refresh_tokens = oAuthTokenProperties.GetRefreshToken();
        request.AddQueryParameter("client_id", client_id);
        request.AddQueryParameter("client_secret", clientSecret);
        request.AddQueryParameter("grant_type", "refresh_token");
        request.AddQueryParameter("refresh_token", refresh_tokens);

        restClient.BaseUrl = new System.Uri("https://oauth2.googleapis.com/token");
        var response = restClient.Post(request);
        _logger.LogCritical(response.Content.ToString());
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            JObject newTokens = JObject.Parse(response.Content);
            if (newTokens != null)
            {
           
                if (newTokens["access_token"] != null)
                {
                    _logger.LogInformation("Access token = "+ newTokens["access_token"].ToString());
                    
                    oAuthTokenProperties.overrideAccessToken(newTokens["access_token"].ToString());
                    
                    var accessToken = oAuthTokenProperties.GetAccessToken(); 
                    while (accessToken == null)
                    {
                        accessToken = oAuthTokenProperties.GetAccessToken();
                    }
                    _logger.LogInformation("Access token was saved");
                }
            }
            // newTokens["refresh_token"] = refresh_tokens;
          //  System.IO.File.WriteAllText(tokenFile,newTokens.ToString());
            return true;
        }
        return false;
    }

}