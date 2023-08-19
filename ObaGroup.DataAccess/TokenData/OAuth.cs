using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using ObaGoupDataAccess.Repository.IRepository;
using ObaGroupUtility;
using RestSharp;

namespace ObaGoupDataAccess;

public class OAuth : IOauth
{
    private static readonly ILogger _logger =
        LoggerFactory.Create(builder => { builder.AddConsole(); }).CreateLogger("OAuth");

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IKeyVaultManager _keyVaultManager;
    private readonly IOAuthTokenProperties _oAuthTokenProperties;
    private readonly IUnitOfWork _unitOfWork;

    public OAuth(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IKeyVaultManager keyVaultManager,
        IOAuthTokenProperties oAuthTokenProperties)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
        _oAuthTokenProperties = oAuthTokenProperties;
        _keyVaultManager = keyVaultManager;
    }

    public bool RefreshTokens()
    {
        var googleCalenderGrantPermissionClientSecret = _keyVaultManager.GetGoogleCalenderGrantPermissionClientSecret();
        var googleCalenderGrantPermissionClientId = _keyVaultManager.GetGoogleCalenderGrantPermissionClientId();


        var restClient = new RestClient();
        var request = new RestSharp.RestRequest();

        _logger.LogInformation("checking if refresh token exist");

        var refresh_tokens = _oAuthTokenProperties.GetRefreshToken();
        request.AddQueryParameter("client_id", googleCalenderGrantPermissionClientId);
        request.AddQueryParameter("client_secret", googleCalenderGrantPermissionClientSecret);
        request.AddQueryParameter("grant_type", "refresh_token");
        request.AddQueryParameter("refresh_token", refresh_tokens);

        restClient.BaseUrl = new Uri(Constants.Google_Get_Token_Endpoint);
        var response = restClient.Post(request);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var newTokens = JObject.Parse(response.Content);
            if (newTokens != null)
                if (newTokens["access_token"] != null)
                {
                    _oAuthTokenProperties.OverrideAccessToken(newTokens["access_token"].ToString());

                    var accessToken = _oAuthTokenProperties.GetAccessToken();
                    while (accessToken == null) accessToken = _oAuthTokenProperties.GetAccessToken();
                    _logger.LogInformation("Access token was saved");
                }

            return true;
        }

        return false;
    }
}