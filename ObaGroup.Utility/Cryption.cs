using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;

namespace ObaGroupUtility;

public class Cryption:Icryption
{

    private IKeyVaultManager _keyVaultManager;
    private string key;
    private string iv;

    public Cryption(IKeyVaultManager keyVaultManager)
    {
        _keyVaultManager = keyVaultManager;
        key = _keyVaultManager.GetAesKey();
        iv = _keyVaultManager.GetAesIv();
    }
    
    private static readonly ILogger _logger = LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
    }).CreateLogger("Cryption");
   public  string Encrypt(string plainText)
    {
        try
        {
            using Aes aesAlg = Aes.Create();
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.IV = Encoding.UTF8.GetBytes(iv);

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using MemoryStream msEncrypt = new MemoryStream();
            using CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }

            byte[] encryptedBytes = msEncrypt.ToArray();
            return Convert.ToBase64String(encryptedBytes);
        }
        catch (Exception e)
        {
            _logger.LogError("Could not encrypt data");
            return "";
        }
    }

   public string Decrypt(string cipherText)
    {
        try
        {
            using Aes aesAlg = Aes.Create();
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.IV = Encoding.UTF8.GetBytes(iv);

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText));
            using CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using StreamReader srDecrypt = new StreamReader(csDecrypt);

            return srDecrypt.ReadToEnd();
        }
        catch (Exception e)
        {
            _logger.LogError("Could not decrypt data");
            return null;
        }
    }
}