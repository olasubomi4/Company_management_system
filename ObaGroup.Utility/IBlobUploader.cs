using Microsoft.AspNetCore.Http;

namespace ObaGroupUtility;

public interface IBlobUploader
{
    string UploadDocument(IFormFile formFile, string blobName);
    string UploadBiographyImage(IFormFile formFile, string blobName);
    string UploadBiographyVideo(IFormFile formFile, string blobName);
    public string UploadProfileImage(IFormFile formFile, string blobName);

    public void DeleteDocument(string blobName);
}