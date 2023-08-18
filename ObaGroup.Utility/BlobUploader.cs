using System.Text.RegularExpressions;
using Azure.Storage;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;

namespace ObaGroupUtility;

public class BlobUploader:IBlobUploader
{
    private IKeyVaultManager _keyVaultManager;
    private string DocumentContainer = "documents";
    private string BiographyImages = "biographyimages";
    private string BiographyVideos = "biographyvideos";
    private string ProfileImages = "profileimages";
    private BlobServiceClient blobServiceClient;
    public BlobUploader(IKeyVaultManager keyVaultManager)
    {
        _keyVaultManager = keyVaultManager;
        string storageAccountName = _keyVaultManager.GetStorageAccountName();
        string accessKey = _keyVaultManager.GetBlobAccessKey();
        var blobUri = $"https://{storageAccountName}.blob.core.windows.net";
        
        var credential = new StorageSharedKeyCredential(storageAccountName, accessKey);
        blobServiceClient= new BlobServiceClient(new Uri(blobUri),credential);
    }
    
    public string UploadDocument(IFormFile formFile, string blobName)
    { 

        BlobContainerClient blobContainer = blobServiceClient.GetBlobContainerClient(DocumentContainer);
        return UploadBlob(formFile, blobName, blobContainer);
    }
    public string UploadBiographyImage(IFormFile formFile, string blobName)
    { 
        BlobContainerClient blobContainer = blobServiceClient.GetBlobContainerClient(BiographyImages);
        return UploadBlob(formFile, blobName, blobContainer);
    }
    public string UploadBiographyVideo(IFormFile formFile, string blobName)
    { 
        BlobContainerClient blobContainer = blobServiceClient.GetBlobContainerClient(BiographyVideos);
        return UploadBlob(formFile, blobName, blobContainer);
    }
    public string UploadProfileImage(IFormFile formFile, string blobName)
    { 
        BlobContainerClient blobContainer = blobServiceClient.GetBlobContainerClient(ProfileImages);
        return UploadBlob(formFile, blobName, blobContainer);
    }
    private string UploadBlob(IFormFile formFile,string blobName,BlobContainerClient blobContainer)
    {
        blobName = ModifyBlobName(blobName);
        BlobClient blobClient = blobContainer.GetBlobClient(blobName);
        blobClient.Upload(formFile.OpenReadStream(), true);
        return blobClient.Uri.ToString();
    }

    private string ModifyBlobName(string blobName)
    {

        int desiredLength = 50; // Set your desired total length here
        int uuidLength = 10;

        blobName = RemoveSpecialCharcters(blobName);
        blobName = RemoveWhiteSpaceAndConvertTextToLowerCase(blobName);
        blobName = blobName.Length > desiredLength - uuidLength
            ? blobName.Substring(0, desiredLength - uuidLength)
            : blobName;

        // Generate a UUID
        string uuid = Guid.NewGuid().ToString("N").Substring(0, uuidLength);

         blobName = blobName + uuid;

        return blobName;
    }

    private string RemoveSpecialCharcters(string blobName)
    {
        blobName = Regex.Replace(blobName, "[^a-zA-Z0-9]", "");
        return blobName;
    }

    private string RemoveWhiteSpaceAndConvertTextToLowerCase(string blobName)
    {
         blobName = Regex.Replace(blobName, @"\s+", "");
         return blobName.ToLower();
    }
    
    public void DeleteDocument(string blobName)
    { 
        
        BlobContainerClient blobContainer = blobServiceClient.GetBlobContainerClient(DocumentContainer);
         DeleteBlob(blobName, blobContainer);
    }
    private void DeleteBlob(string blobName,BlobContainerClient blobContainer)
    {
        BlobClient blobClient = blobContainer.GetBlobClient(blobName);
        int status = blobClient.Delete().Status;
    }

    
}