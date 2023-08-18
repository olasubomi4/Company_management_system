using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Logging;

namespace ObaGroupUtility;

public class KeyVaultManager:IKeyVaultManager

{

    private readonly SecretClient _secretClient;
    private string _googleCalendarApiKey;
    private string _googleCalendarToken;
    private string _googleSignInClientId;
    private string _googleSignInClientSecret;
    private string _googleCalenderGrantPermissionClientId;
    private string _googleCalenderGrantPermissionClientSecret;
    private string _aesKey;
    private string _aesIv;
    private string _storageAccountName;
    private string _blobAccessKey;
    private string _broadCastingMail;
    private string _broadCastingMailPassword;
    
    private static readonly ILogger _logger = LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
    }).CreateLogger("KeyVault");

    public KeyVaultManager(SecretClient secretClient)
    {
        _secretClient = secretClient;
    }

    public string GetGoogleCalenderGrantPermissionClientSecret()
    {
        if (string.IsNullOrEmpty(_googleCalenderGrantPermissionClientSecret))
        {
            _googleCalenderGrantPermissionClientSecret =GetSecret("GoogleCalenderGrantPermissionClientSecret").Result.ToString();
        }

        return _googleCalenderGrantPermissionClientSecret;
    }

    public string GetBrodCastingMail()
    {
        if (string.IsNullOrEmpty(_broadCastingMail))
        {
            _broadCastingMail =GetSecret("BroadCastingMail").Result.ToString();
        }

        return _broadCastingMail;
    }
    
    public string GetBrodCastingMailPassword()
    {
        if (string.IsNullOrEmpty(_broadCastingMailPassword))
        {
            _broadCastingMailPassword =GetSecret("BroadCastingMailPassword").Result.ToString();
        }

        return _broadCastingMailPassword;
    }
    public string GetGoogleCalendarApiKey()
    {
        if (string.IsNullOrEmpty(_googleCalendarApiKey))
        {
            _googleCalendarApiKey =GetSecret("GoogleCalendarApiKey").Result.ToString();
        }

        return _googleCalendarApiKey;
    }
    
    public string GetAesKey()
    {
        if (string.IsNullOrEmpty(_aesKey))
        {
            
            _aesKey= GetSecret("AESKEY").Result.ToString();
        }

        return _aesKey;
    }
    
    public string GetAesIv()
    {
        if (string.IsNullOrEmpty(_aesIv))
        {
            _aesIv= GetSecret("AESID").Result.ToString();
        }

        return _aesIv;
    }

    public string GetGoogleSignInClientId()
    {
        if (string.IsNullOrEmpty(_googleSignInClientId))
        {
            _googleSignInClientId= GetSecret("GoogleSignInClientId").Result.ToString();
        }
        return _googleSignInClientId;
    }

    public string GetGoogleSignInClientSecret()
    {
        if (string.IsNullOrEmpty(_googleSignInClientSecret))
        {
            _googleSignInClientSecret= GetSecret("GoogleSignInClientSecret").Result.ToString();
        }
        return _googleSignInClientSecret;
    }

    public string GetBlobAccessKey()
    {
        if (string.IsNullOrEmpty(_blobAccessKey))
        {
            _blobAccessKey= GetSecret("BlobAccessKey").Result.ToString();
        }
        return _blobAccessKey;
    }
    
    public string GetStorageAccountName()
    {
        if (string.IsNullOrEmpty(_storageAccountName))
        {
            _storageAccountName= GetSecret("StorageAccountName").Result.ToString();
        }

        return _storageAccountName;
    }
    
    public string GetGoogleCalendarToken()
    {
        if (string.IsNullOrEmpty(_googleCalendarToken))
        {
            _googleCalendarToken= GetSecret("GoogleCalendarToken").Result.ToString();
        }
        return _googleCalendarToken;
    }

    public string GetGoogleCalenderGrantPermissionClientId()
    {
        if (string.IsNullOrEmpty(_googleCalenderGrantPermissionClientId))
        {
            _googleCalenderGrantPermissionClientId= GetSecret("GoogleCalenderGrantPermissionClientId").Result.ToString();
        }

        return _googleCalenderGrantPermissionClientId;
    }
    
    
    
    private async Task<string> GetSecret(string secretName)

    {
        try

        {
            Console.WriteLine("gettinf keys for "+secretName);
            KeyVaultSecret keyValueSecret = await _secretClient.GetSecretAsync(secretName);
            return keyValueSecret.Value;
        }

        catch (Exception e)
        {
            _logger.LogError("Could not get " + secretName+" from azure due to "+ e);
            return null;
        }

    }

    public async Task<string> UpsertSecret(string secretName,string secretValue)
    {
        try
        {
            KeyVaultSecret keyValueSecret = await _secretClient.SetSecretAsync(secretName,secretValue);
            return keyValueSecret.Value;
        }

        catch (Exception e)
        {
            _logger.LogError("Could not update " + secretName+" to azure due to "+ e);
            return null;
        }
    }


}

