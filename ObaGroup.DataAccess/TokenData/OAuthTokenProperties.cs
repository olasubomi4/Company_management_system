using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ObaGoupDataAccess.Data;
using ObaGoupDataAccess.Repository;
using ObaGoupDataAccess.Repository.IRepository;
using ObaGroupModel;
using ObaGroupUtility;
using RestSharp;

namespace ObaGoupDataAccess;

public class OAuthTokenProperties:IOAuthTokenProperties
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    private readonly Icryption _cryption;

    
    private static readonly ILogger _logger = LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
    }).CreateLogger("OAuthTokenProperties");
    
    public OAuthTokenProperties(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork,Icryption cryption)
    {
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = unitOfWork;
        _cryption = cryption;
    }
    
    public string GetAccessToken()
    {
        _logger.LogInformation("GETTING ACCESS TOKEN");
        var token = new GoogleTokensUtility(_httpContextAccessor,_cryption).GetAccessToken();
       return token;
    }
    public int GetExpires_in()
    {
        var context = _httpContextAccessor.HttpContext;
        return  Int32.Parse(context.Request.Cookies["OauthExpiresIn"] ?? "0");
    }
    
    public string GetRefreshToken()
    {
        _logger.LogInformation("GETTING REFRESH TOKEN");
        var user = _httpContextAccessor.HttpContext.User;
        var userEmail = user.FindFirst(ClaimTypes.Email)?.Value;
        _logger.LogInformation("USER EMAIL : "+userEmail);
        UserOauthRefreshToken userOauthRefreshToken =_unitOfWork.UserOauthRefreshTokenRepository.GetFirstOrDefault(u => u.ApplicationUser.Email == userEmail);

        if (userOauthRefreshToken == null)
        {
            return null;
        }
        string? refreshToken= null;
        if (!string.IsNullOrEmpty(userOauthRefreshToken.RefreshToken))
        {
             refreshToken=_cryption.Decrypt(userOauthRefreshToken.RefreshToken);
        }
       
   
        return refreshToken;
    }

    public void RevokeAppAccessToCalendar(string accessToken)
    {
        RestClient restClient = new RestClient();
        RestSharp.RestRequest request = new RestSharp.RestRequest();
        
        request.AddQueryParameter("token", accessToken);

        restClient.BaseUrl = new System.Uri(Constants.Google_Revoke_Token_Endpoint);
        var response = restClient.Post(request);
        
        Console.WriteLine(response.Content);
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            RevokeToken(accessToken);
        }
    }
    public void RevokeToken(string accessToken)
    {
        _logger.LogInformation("REVOKING TOKEN");
        
        new GoogleTokensUtility(_httpContextAccessor,_cryption).SetAccessToken(accessToken);
        new GoogleTokensUtility(_httpContextAccessor,_cryption).DeleteOauthTokenId();
        var user = _httpContextAccessor.HttpContext.User;
        var userEmail = user.FindFirst(ClaimTypes.Email)?.Value;
        var userOauthRefreshTokenObj=_unitOfWork.UserOauthRefreshTokenRepository.GetFirstOrDefault(u => u.ApplicationUser.Email == userEmail);
        if (userOauthRefreshTokenObj != null)
        {
           _unitOfWork.UserOauthRefreshTokenRepository.Remove(userOauthRefreshTokenObj);
           _unitOfWork.Save();
        }
        _logger.LogInformation("DONE REVOKING TOKEN");
    }
    
    public void OverrideAccessToken(string accessTokenFromGoogle)
    {        
        _logger.LogInformation("OVERRIDING ACCESS TOKEN");
        string accessToken = accessTokenFromGoogle;
        new GoogleTokensUtility(_httpContextAccessor,_cryption).SetAccessToken(accessToken);
        _logger.LogInformation("DONE OVERRIDING ACCESS TOKEN");
    }
}