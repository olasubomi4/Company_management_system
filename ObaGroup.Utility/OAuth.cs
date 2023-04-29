using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace ObaGroupUtility;

public static class OAuth
{
    private static readonly ILogger _logger = LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
    }).CreateLogger("OAuth");

    public static Boolean RefreshTokens()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var tokenFile = currentDirectory+"/tokens.json";
        var credentialsFile =
            currentDirectory+"/client_secret_87857337556-iqm8t560cfhc8ddln4mdk88ahl311na9.apps.googleusercontent.com.json";
        var credentials = JObject.Parse((System.IO.File.ReadAllText(credentialsFile)));
        var tokens = JObject.Parse((System.IO.File.ReadAllText(tokenFile)));

        RestClient restClient = new RestClient();
        RestSharp.RestRequest request = new RestSharp.RestRequest();
        
        var client_id=credentials["web"]["client_id"].ToString();
        var clientSecret = credentials["web"]["client_secret"].ToString();
        
        _logger.LogInformation("checking if refresh token exist");

        var refresh_tokens = tokens["refresh_token"].ToString();
        request.AddQueryParameter("client_id", client_id);
        request.AddQueryParameter("client_secret", clientSecret);
        request.AddQueryParameter("grant_type", "refresh_token");
        request.AddQueryParameter("refresh_token", refresh_tokens);

        restClient.BaseUrl = new System.Uri("https://oauth2.googleapis.com/token");
        var response = restClient.Post(request);
        Console.WriteLine("refreshing Token");
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            JObject newTokens = JObject.Parse(response.Content);
            newTokens["refresh_token"] = refresh_tokens;
            System.IO.File.WriteAllText(tokenFile,newTokens.ToString());
            return true;
        }
        return false;
    }
    
}