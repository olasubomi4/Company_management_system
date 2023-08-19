namespace ObaGroupUtility;

public interface IKeyVaultManager
{
    // public Task<string> GetSecret(string secretName);
    public Task<string> UpsertSecret(string secretName, string secretValue);

    public string GetGoogleCalendarToken();

    public string GetGoogleCalenderGrantPermissionClientId();
    public string GetGoogleCalenderGrantPermissionClientSecret();
    public string GetGoogleCalendarApiKey();
    public string GetAesKey();
    public string GetAesIv();
    string GetBrodCastingMailPassword();
    string GetBrodCastingMail();
    public string GetDbConnectionString();

    public string GetGoogleSignInClientId();
    public string GetGoogleSignInClientSecret();

    public string GetBlobAccessKey();
    public string GetStorageAccountName();
}