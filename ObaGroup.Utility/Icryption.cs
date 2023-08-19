namespace ObaGroupUtility;

public interface Icryption
{
    string Encrypt(string plainText);
    string Decrypt(string cypherText);
}