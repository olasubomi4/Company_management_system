using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Logging;

namespace ObaGroupUtility;

public class KeyVaultManager : IKeyVaultManager

{
    private static readonly ILogger _logger =
        LoggerFactory.Create(builder => { builder.AddConsole(); }).CreateLogger("KeyVault");

    private readonly SecretClient _secretClient;
    private string _aesIv;
    private string _aesKey;
    private string _blobAccessKey;
    private string _broadCastingMail;
    private string _broadCastingMailPassword;
    private string _dbConnectionString;
    private string _googleCalendarApiKey;
    private string _googleCalendarToken;
    private string _googleCalenderGrantPermissionClientId;
    private string _googleCalenderGrantPermissionClientSecret;
    private string _googleSignInClientId;
    private string _googleSignInClientSecret;
    private string _storageAccountName;

    public KeyVaultManager(SecretClient secretClient)
    {
        _secretClient = secretClient;
    }

    public string GetGoogleCalenderGrantPermissionClientSecret()
    {
        if (string.IsNullOrEmpty(_googleCalenderGrantPermissionClientSecret))
            _googleCalenderGrantPermissionClientSecret = GetSecret("GoogleCalenderGrantPermissionClientSecret").Result;

        return _googleCalenderGrantPermissionClientSecret;
    }

    public string GetDbConnectionString()
    {
        if (string.IsNullOrEmpty(_dbConnectionString)) _dbConnectionString = GetSecret("DbConnectionString").Result;

        return _dbConnectionString;
    }

    public string GetBrodCastingMail()
    {
        if (string.IsNullOrEmpty(_broadCastingMail)) _broadCastingMail = GetSecret("BroadCastingMail").Result;

        return _broadCastingMail;
    }

    public string GetBrodCastingMailPassword()
    {
        if (string.IsNullOrEmpty(_broadCastingMailPassword))
            _broadCastingMailPassword = GetSecret("BroadCastingMailPassword").Result;

        return _broadCastingMailPassword;
    }

    public string GetGoogleCalendarApiKey()
    {
        if (string.IsNullOrEmpty(_googleCalendarApiKey))
            _googleCalendarApiKey = GetSecret("GoogleCalendarApiKey").Result;

        return _googleCalendarApiKey;
    }

    public string GetAesKey()
    {
        if (string.IsNullOrEmpty(_aesKey)) _aesKey = GetSecret("AESKEY").Result;

        return _aesKey;
    }

    public string GetAesIv()
    {
        if (string.IsNullOrEmpty(_aesIv)) _aesIv = GetSecret("AESID").Result;

        return _aesIv;
    }

    public string GetGoogleSignInClientId()
    {
        if (string.IsNullOrEmpty(_googleSignInClientId))
            _googleSignInClientId = GetSecret("GoogleSignInClientId").Result;
        return _googleSignInClientId;
    }

    public string GetGoogleSignInClientSecret()
    {
        if (string.IsNullOrEmpty(_googleSignInClientSecret))
            _googleSignInClientSecret = GetSecret("GoogleSignInClientSecret").Result;
        return _googleSignInClientSecret;
    }

    public string GetBlobAccessKey()
    {
        if (string.IsNullOrEmpty(_blobAccessKey)) _blobAccessKey = GetSecret("BlobAccessKey").Result;
        return _blobAccessKey;
    }

    public string GetStorageAccountName()
    {
        if (string.IsNullOrEmpty(_storageAccountName)) _storageAccountName = GetSecret("StorageAccountName").Result;

        return _storageAccountName;
    }

    public string GetGoogleCalendarToken()
    {
        if (string.IsNullOrEmpty(_googleCalendarToken)) _googleCalendarToken = GetSecret("GoogleCalendarToken").Result;
        return _googleCalendarToken;
    }

    public string GetGoogleCalenderGrantPermissionClientId()
    {
        if (string.IsNullOrEmpty(_googleCalenderGrantPermissionClientId))
            _googleCalenderGrantPermissionClientId = GetSecret("GoogleCalenderGrantPermissionClientId").Result;

        return _googleCalenderGrantPermissionClientId;
    }

    public async Task<string> UpsertSecret(string secretName, string secretValue)
    {
        try
        {
            _logger.LogInformation("Updating " + secretName);
            KeyVaultSecret keyValueSecret = await _secretClient.SetSecretAsync(secretName, secretValue);
            return keyValueSecret.Value;
        }

        catch (Exception e)
        {
            _logger.LogError("Could not update " + secretName + " to azure due to " + e);
            return null;
        }
    }


    private async Task<string> GetSecret(string secretName)

    {
        try

        {
            _logger.LogInformation("getting keys for " + secretName);
            KeyVaultSecret keyValueSecret = await _secretClient.GetSecretAsync(secretName);
            return keyValueSecret.Value;
        }

        catch (Exception e)
        {
            _logger.LogError("Could not get " + secretName + " from azure due to " + e);
            return null;
        }
    }
}