namespace ObaGoupDataAccess;

public interface IOAuthTokenProperties
{
    string GetAccessToken();
    int GetExpires_in();
    string GetRefreshToken();
    void RevokeAppAccessToCalendar(string accessToken);
    void RevokeToken(string accessToken);
    void OverrideAccessToken(string accessTokenFromGoogle);
}