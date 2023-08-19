using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ObaGoupDataAccess;
using ObaGoupDataAccess.Repository.IRepository;
using ObaGroupModel;
using ObaGroupUtility;
using RestSharp;

namespace Oba_group2.Areas.Admin.Controllers;

public class OauthController : Controller
{
    private readonly Icryption _cryption;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IKeyVaultManager _keyVaultManager;
    private readonly IOAuthTokenProperties _oAuthTokenProperties;
    private readonly IUnitOfWork _unitOfWork;

    public OauthController(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork,
        IKeyVaultManager keyVaultManager, Icryption cryption, IOAuthTokenProperties oAuthTokenProperties)
    {
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = unitOfWork;
        _keyVaultManager = keyVaultManager;
        _cryption = cryption;
        _oAuthTokenProperties = oAuthTokenProperties;
    }

    [Route(Constants.Google_Calendar_Callback_Endpoint)]
    public IActionResult callback(string code, string? error, string? state)
    {
        var responseModel = new ResponseModel();
        if (string.IsNullOrWhiteSpace(error))
        {
            var uri = GetTokens(code);
            return Redirect(uri);
        }

        return Redirect(failedAttempt(error));
    }

    public string failedAttempt(string error)
    {
        return $"{Request.Scheme}://{Request.Host}{Constants.Create_Event_Endpoint}?error=" + error;
    }

    [HttpPost]
    public string GetTokens(string code)
    {
        var restClient = new RestClient();
        var request = new RestSharp.RestRequest();

        var googleCalenderGrantPermissionClientSecret = _keyVaultManager.GetGoogleCalenderGrantPermissionClientSecret();
        var googleCalenderGrantPermissionClientId = _keyVaultManager.GetGoogleCalenderGrantPermissionClientId();

        request.AddQueryParameter("client_id", googleCalenderGrantPermissionClientId);
        request.AddQueryParameter("client_secret", googleCalenderGrantPermissionClientSecret);
        request.AddQueryParameter("code", code);
        request.AddQueryParameter("grant_type", "authorization_code");
        request.AddQueryParameter("redirect_uri",
            $"{Request.Scheme}://{Request.Host}{Constants.Google_Calendar_Callback_Endpoint}");

        restClient.BaseUrl = new Uri(Constants.Google_Get_Token_Endpoint);
        var response = restClient.Post(request);
        var responseContent = response.Content;

        var token = JObject.Parse(responseContent);
        var oauthToken = new OauthToken();
        if (token != null)
        {
            if (token["access_token"] != null) oauthToken.access_token = token["access_token"].ToString();
            if (token["expires_in"] != null) oauthToken.expires_in = int.Parse(token["expires_in"].ToString());
            if (token["scope"] != null) oauthToken.scope = token["scope"].ToString();
            if (token["token_type"] != null) oauthToken.token_type = token["token_type"].ToString();
            if (token["id_token"] != null) oauthToken.id_token = token["id_token"].ToString();
            if (token["refresh_token"] != null)
                oauthToken.refresh_token = token["refresh_token"].ToString();
            else
                _oAuthTokenProperties.RevokeAppAccessToCalendar(oauthToken.access_token);
        }

        Console.WriteLine(oauthToken);

        SetCsrfToken(oauthToken);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            Console.WriteLine("GETTING VALUE FROM METHOD");
            Console.WriteLine(_oAuthTokenProperties.GetAccessToken());

            _keyVaultManager.UpsertSecret("GoogleCalendarToken", response.Content);

            Console.WriteLine("hello");
            Console.WriteLine(response.Content);
            return $"{Request.Scheme}://{Request.Host}{Constants.Create_Event_Endpoint}";
        }

        return failedAttempt(response.Content);
    }


    [HttpPost]
    public IActionResult RevokeTokens()
    {
        var access_token = _oAuthTokenProperties.GetAccessToken();
        Console.WriteLine(access_token);
        var restClient = new RestClient();
        var request = new RestSharp.RestRequest();

        request.AddQueryParameter("token", access_token);

        restClient.BaseUrl = new Uri(Constants.Google_Revoke_Token_Endpoint);
        var response = restClient.Post(request);

        Console.WriteLine(response.Content);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            _oAuthTokenProperties.RevokeToken(access_token);
            return Ok(response.Content);
        }

        return BadRequest(response.Content);
    }

    public void SetCsrfToken(OauthToken oauthToken)
    {
        if (oauthToken.access_token != null)
            new GoogleTokensUtility(_httpContextAccessor, _cryption).SetAccessToken(oauthToken.access_token);

        if (oauthToken.id_token != null)
            new GoogleTokensUtility(_httpContextAccessor, _cryption).SetOauthTokenId(oauthToken.id_token);

        if (oauthToken.token_type != null)
            new GoogleTokensUtility(_httpContextAccessor, _cryption).SetOauthTokenType(oauthToken.token_type);

        if (oauthToken.refresh_token != null)
        {
            var user = _httpContextAccessor.HttpContext.User;
            var userEmail = user.FindFirst(ClaimTypes.Email)?.Value;

            var applicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Email == userEmail);
            var userOauthRefreshToken =
                _unitOfWork.UserOauthRefreshTokenRepository.GetFirstOrDefault(u =>
                    u.ApplicationUser == applicationUser);

            if (userOauthRefreshToken != null)
            {
                userOauthRefreshToken.RefreshToken = _cryption.Encrypt(oauthToken.refresh_token);
                _unitOfWork.UserOauthRefreshTokenRepository.Update(userOauthRefreshToken);
            }
            else
            {
                userOauthRefreshToken = new UserOauthRefreshToken();
                userOauthRefreshToken.RefreshToken = _cryption.Encrypt(oauthToken.refresh_token);
                userOauthRefreshToken.ApplicationUser = applicationUser;
                _unitOfWork.UserOauthRefreshTokenRepository.Add(userOauthRefreshToken);
            }

            _unitOfWork.Save();
        }

        if (oauthToken.expires_in != null)
            new GoogleTokensUtility(_httpContextAccessor, _cryption).SetOauthTokenExpiresIn(oauthToken.expires_in
                .ToString());

        if (oauthToken.scope != null)
            new GoogleTokensUtility(_httpContextAccessor, _cryption).SetOauthTokenScope(oauthToken.scope);
    }
}