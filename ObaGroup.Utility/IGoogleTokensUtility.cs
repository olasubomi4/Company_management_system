namespace ObaGroupUtility;

public interface IGoogleTokensUtility
{
    string GetAccessToken();
    void SetAccessToken(string newAccessToken);
    string GetOauthTokenId();
    void DeleteOauthTokenId();
    string GetOauthTokenType();
    void SetOauthTokenType(string newOauthTokenType);

    string GetOauthTokenExpiresIn();
    void SetOauthTokenExpiresIn(string newOauthTokenExpiresIn);
    string GetOauthTokenScope();
    void SetOauthTokenScope(string newOauthTokenScope);

}