using Microsoft.AspNetCore.Http;

namespace ObaGroupUtility;

public class GoogleTokensUtility : IGoogleTokensUtility
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly Icryption _cryption;

    public GoogleTokensUtility(IHttpContextAccessor httpContextAccessor, Icryption cryption)
    {
        _httpContextAccessor = httpContextAccessor;
        _cryption = cryption;
    }

    public string GetAccessToken()
    {
        return _cryption.Decrypt(_httpContextAccessor.HttpContext.Session.GetString("OauthTokenAccessToken")) ?? "";
    }

    public void SetAccessToken(string newAccessToken)
    {
        _httpContextAccessor.HttpContext.Session.SetString("OauthTokenAccessToken", _cryption.Encrypt(newAccessToken));
    }


    public string GetOauthTokenId()
    {
        return _cryption.Decrypt(_httpContextAccessor.HttpContext.Session.GetString("OauthTokenIdToken")) ?? "";
    }

    public void DeleteOauthTokenId()
    {
        _httpContextAccessor.HttpContext.Session.Remove("OauthTokenIdToken");
    }

    public string GetOauthTokenType()
    {
        return _cryption.Decrypt(_httpContextAccessor.HttpContext.Session.GetString("OauthTokenType")) ?? "";
    }

    public void SetOauthTokenType(string newOauthTokenType)
    {
        _httpContextAccessor.HttpContext.Session.SetString("OauthTokenType", _cryption.Encrypt(newOauthTokenType));
    }

    public string GetOauthTokenExpiresIn()
    {
        return _cryption.Decrypt(_httpContextAccessor.HttpContext.Session.GetString("OauthExpiresIn")) ?? "";
    }

    public void SetOauthTokenExpiresIn(string newOauthTokenExpiresIn)
    {
        _httpContextAccessor.HttpContext.Session.SetString("OauthExpiresIn", _cryption.Encrypt(newOauthTokenExpiresIn));
    }

    public string GetOauthTokenScope()
    {
        return _cryption.Decrypt(_httpContextAccessor.HttpContext.Session.GetString("oauthTokenScope")) ?? "";
    }

    public void SetOauthTokenScope(string newOauthTokenScope)
    {
        _httpContextAccessor.HttpContext.Session.SetString("oauthTokenScope", _cryption.Encrypt(newOauthTokenScope));
    }

    public void SetOauthTokenId(string newOauthTokenId)
    {
        _httpContextAccessor.HttpContext.Session.SetString("OauthTokenIdToken", _cryption.Encrypt(newOauthTokenId));
    }
}