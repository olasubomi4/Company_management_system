using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;

namespace ObaGroupUtility;

public class Cryption : Icryption
{
    private static readonly ILogger _logger =
        LoggerFactory.Create(builder => { builder.AddConsole(); }).CreateLogger("Cryption");

    private readonly IKeyVaultManager _keyVaultManager;
    private readonly string iv;
    private readonly string key;

    public Cryption(IKeyVaultManager keyVaultManager)
    {
        _keyVaultManager = keyVaultManager;
        key = _keyVaultManager.GetAesKey();
        iv = _keyVaultManager.GetAesIv();
    }

    public string Encrypt(string plainText)
    {
        try
        {
            using var aesAlg = Aes.Create();
          
            aesAlg.Key = Convert.FromBase64String(key);
            aesAlg.IV = Convert.FromBase64String(iv);

            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using var msEncrypt = new MemoryStream();
            using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }

            var encryptedBytes = msEncrypt.ToArray();
            return Convert.ToBase64String(encryptedBytes);
        }
        catch (Exception e)
        {
            _logger.LogError("Could not encrypt data because"+e);
            return "";
        }
    }

    public string Decrypt(string cipherText)
    {
        try
        {
            using var aesAlg = Aes.Create();
        
            aesAlg.Key = Convert.FromBase64String(key);
            aesAlg.IV = Convert.FromBase64String(iv);

            var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using var msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText));
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);

            return srDecrypt.ReadToEnd();
        }
        catch (Exception e)
        {
            _logger.LogError("Could not decrypt data");
            return null;
        }
    }
}