using Microsoft.AspNetCore.Http;

namespace ObaGroupUtility;

public interface IBlobUploader
{
    string UploadDocument(IFormFile formFile, string blobName,string extension);
    string UploadBiographyImage(IFormFile formFile, string blobName,string extension);
    string UploadBiographyVideo(IFormFile formFile, string blobName,string extension);
    public string UploadProfileImage(IFormFile formFile, string blobName,string extension);

    public void DeleteDocument(string blobName);
}