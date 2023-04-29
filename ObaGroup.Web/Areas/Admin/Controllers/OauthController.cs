using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ObaGroupModel;
using RestSharp;


namespace Oba_group2.Areas.Admin.Controllers;

public  class OauthController : Controller
{
    private readonly string currentDirectory = Directory.GetCurrentDirectory();
    
    [Route("oauth/callback")]
    public IActionResult callback(string code, string? error, string? state)
    {
        ResponseModel responseModel = new ResponseModel();
        if (string.IsNullOrWhiteSpace(error))
        {
            Console.WriteLine("again");
           string uri= this.GetTokens(code);
           return Redirect(uri);
        }
        return Redirect(failedAttempt(error));
    }

    public string failedAttempt(string error)
    {
        return "https://localhost:7151/Admin/Dashboard/Calendar/google?error="+error;
    }
    
    [HttpPost]
    public string GetTokens(string code)
    {
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
        request.AddQueryParameter("redirect_uri", "https://localhost:7151/oauth/callback");

        restClient.BaseUrl = new System.Uri("https://oauth2.googleapis.com/token");
        var response = restClient.Post(request);

        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            System.IO.File.WriteAllText(tokenFile,response.Content);
            Console.WriteLine("hello");
            Console.WriteLine(response.Content);
            string uri= "https://localhost:7151/Admin/Dashboard/Calendar/google";
            return "https://localhost:7151/Admin/Dashboard/Calendar/google";
        }
        return failedAttempt(response.Content);
    }
    
    
    [HttpPost]
    [Route("oauth/revoke")]
    public IActionResult RevokeTokens()
    {
        var tokenFile = currentDirectory+"/tokens.json";
        var credentialsFile = currentDirectory+"/client_secret_87857337556-iqm8t560cfhc8ddln4mdk88ahl311na9.apps.googleusercontent.com.json";
        var tokens = JObject.Parse((System.IO.File.ReadAllText(tokenFile)));

     
        var access_token = tokens["access_token"].ToString();
        Console.WriteLine(access_token);
        RestClient restClient = new RestClient();
        RestSharp.RestRequest request = new RestSharp.RestRequest();
        
        request.AddQueryParameter("token", access_token);

        restClient.BaseUrl = new System.Uri("https://oauth2.googleapis.com/revoke");
        var response = restClient.Post(request);
        
        Console.WriteLine(response.Content);
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            JObject newTokens = JObject.Parse(response.Content);
            newTokens["access_token"] = access_token;
            System.IO.File.WriteAllText(tokenFile,newTokens.ToString());
            return Ok(response.Content);
        }
        return BadRequest(response.Content);
    }
    
    
}