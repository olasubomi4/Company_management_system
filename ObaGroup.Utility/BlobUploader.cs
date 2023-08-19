using System.Text.RegularExpressions;
using Azure.Storage;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;

namespace ObaGroupUtility;

public class BlobUploader : IBlobUploader
{
    private readonly IKeyVaultManager _keyVaultManager;
    private readonly string BiographyImages = "biographyimages";
    private readonly string BiographyVideos = "biographyvideos";
    private readonly BlobServiceClient blobServiceClient;
    private readonly string DocumentContainer = "documents";
    private readonly string ProfileImages = "profileimages";

    public BlobUploader(IKeyVaultManager keyVaultManager)
    {
        _keyVaultManager = keyVaultManager;
        var storageAccountName = _keyVaultManager.GetStorageAccountName();
        var accessKey = _keyVaultManager.GetBlobAccessKey();
        var blobUri = $"https://{storageAccountName}.blob.core.windows.net";

        var credential = new StorageSharedKeyCredential(storageAccountName, accessKey);
        blobServiceClient = new BlobServiceClient(new Uri(blobUri), credential);
    }

    public string UploadDocument(IFormFile formFile, string blobName)
    {
        var blobContainer = blobServiceClient.GetBlobContainerClient(DocumentContainer);
        return UploadBlob(formFile, blobName, blobContainer);
    }

    public string UploadBiographyImage(IFormFile formFile, string blobName)
    {
        var blobContainer = blobServiceClient.GetBlobContainerClient(BiographyImages);
        return UploadBlob(formFile, blobName, blobContainer);
    }

    public string UploadBiographyVideo(IFormFile formFile, string blobName)
    {
        var blobContainer = blobServiceClient.GetBlobContainerClient(BiographyVideos);
        return UploadBlob(formFile, blobName, blobContainer);
    }

    public string UploadProfileImage(IFormFile formFile, string blobName)
    {
        var blobContainer = blobServiceClient.GetBlobContainerClient(ProfileImages);
        return UploadBlob(formFile, blobName, blobContainer);
    }

    public void DeleteDocument(string blobName)
    {
        var blobContainer = blobServiceClient.GetBlobContainerClient(DocumentContainer);
        DeleteBlob(blobName, blobContainer);
    }

    private string UploadBlob(IFormFile formFile, string blobName, BlobContainerClient blobContainer)
    {
        blobName = ModifyBlobName(blobName);
        var blobClient = blobContainer.GetBlobClient(blobName);
        blobClient.Upload(formFile.OpenReadStream(), true);
        return blobClient.Uri.ToString();
    }

    private string ModifyBlobName(string blobName)
    {
        var desiredLength = 50; // Set your desired total length here
        var uuidLength = 10;

        blobName = RemoveSpecialCharcters(blobName);
        blobName = RemoveWhiteSpaceAndConvertTextToLowerCase(blobName);
        blobName = blobName.Length > desiredLength - uuidLength
            ? blobName.Substring(0, desiredLength - uuidLength)
            : blobName;

        // Generate a UUID
        var uuid = Guid.NewGuid().ToString("N").Substring(0, uuidLength);

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

    private void DeleteBlob(string blobName, BlobContainerClient blobContainer)
    {
        var blobClient = blobContainer.GetBlobClient(blobName);
        var status = blobClient.Delete().Status;
    }
}