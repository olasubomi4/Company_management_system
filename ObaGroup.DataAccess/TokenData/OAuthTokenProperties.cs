using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ObaGoupDataAccess.Data;
using ObaGoupDataAccess.Repository;
using ObaGoupDataAccess.Repository.IRepository;
using ObaGroupModel;

namespace ObaGoupDataAccess;

public class OAuthTokenProperties
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    public string accessToken;
    
    private static readonly ILogger _logger = LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
    }).CreateLogger("OAuthTokenProperties");
    
    public OAuthTokenProperties(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
    {
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = unitOfWork;
    }
    
    public string GetAccessToken()
    {
        _logger.LogInformation("GETTING ACCESS TOKEN");
        var context = _httpContextAccessor.HttpContext;
        var token = context.Request.Cookies["OauthTokenAccessToken"] ?? "";
       _logger.LogInformation("TOken ="+token);
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
        var refreshToken=userOauthRefreshToken.RefreshToken;
        _logger.LogInformation("refresh token ="+ refreshToken);
        return refreshToken;
    }
  
  /*  public string GetIdToken()
    {
        _logger.LogInformation("GETTING ID TOKEN");
        var context = _httpContextAccessor.HttpContext;
       // var idToken =context.Response.Cookies["OauthTokenIdToken"];
        _logger.LogInformation("ID token =" + idToken);
        return idToken;
    }
    */
    
    public void RevokeToken(string accessToken)
    {
        _logger.LogInformation("REVOKING TOKEN");
        var context = _httpContextAccessor.HttpContext;
        context.Response.Cookies.Append("OauthTokenAccessToken", accessToken);
        
      //  context.Response.Cookies.Delete("OauthExpiresIN");
        context.Response.Cookies.Delete("OauthTokenIdToken");
        
        var user = _httpContextAccessor.HttpContext.User;
        var userEmail = user.FindFirst(ClaimTypes.Email)?.Value;
        var userOauthRefreshTokenObj=_unitOfWork.UserOauthRefreshTokenRepository.GetFirstOrDefault(u => u.ApplicationUser.Email == userEmail);
        _unitOfWork.UserOauthRefreshTokenRepository.Remove(userOauthRefreshTokenObj);
        
        _logger.LogInformation("DONE REVOKING TOKEN");
    }
    
    public void overrideAccessToken(string accessTokenFromGoogle)
    {        
        _logger.LogInformation("OVERRIDING ACCESS TOKEN = "+accessTokenFromGoogle);
        var context = _httpContextAccessor.HttpContext;
        accessToken = accessTokenFromGoogle;
        context.Response.Cookies.Append("OauthTokenAccessToken", accessToken);
        _logger.LogInformation("DONE OVERRIDING ACCESS TOKEN");
    }
}