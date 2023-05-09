using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObaGoupDataAccess.Repository.IRepository;
using ObaGroupModel;
using ObaGroupModel.Validation;
using ObaGroupUtility;
using System.Text.Json;

namespace Oba_group2.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Constants.Role_Admin)]
public class BiographyController : Controller
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _hostEnvironment;

    public BiographyController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _hostEnvironment = hostEnvironment;
    }

    
    [HttpGet]
    [Route("Admin/Dashboard/Biography/Upsert")]
    public IActionResult GetById(int? id)
    {
        Biography biography = _unitOfWork.biography.GetFirstOrDefault(x=> x.id==id);
        return Ok(biography);
    }

    [HttpGet]
    [Route("Admin/Dashboard/Biography/Upsert/GetAll")]
    public IActionResult GetAll()
    {
        IEnumerable<Biography> biographies = _unitOfWork.biography.GetAll();
        return Ok(biographies);
    }

    [HttpPost]
    [RequestFormLimits(MultipartBodyLengthLimit = 6104857600)]
    [RequestSizeLimit(6104857600)]
    [Route("Admin/Dashboard/Biography/Upsert")]
    public IActionResult Upsert([FromForm] BiographyWithFile biography)
    {
        Boolean isCreate = false;
        string message;
        Biography obj = biography.biography;
        List<string> documentsUrl = new List<string>();
        ResponseModel responseModel = new ResponseModel();


        if (!ModelState.IsValid)
        {
            responseModel.Message = "Bad Request";
            responseModel.StatusCode = 400;
            var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return BadRequest(new { responseModel, Errors = errors2 });
        }
        
        if (obj.id == 0)
        {
            obj.profileImageUrl = UploadImages(biography.profileImage);
            obj.profileVideoUrl = UploadVideos(biography.profileVideo);
            _unitOfWork.biography.Add(obj);
            isCreate = true;
        }
        else
        {
            obj.profileImageUrl = obj.profileImageUrl + UploadImages(biography.profileImage);
            obj.profileVideoUrl = obj.profileVideoUrl + UploadVideos(biography.profileVideo);
            _unitOfWork.biography.Update(obj);
        }

        _unitOfWork.Save();
        if (isCreate)
        {
            message = "Biography created successfully";
        }
        else
        {
            message = "Biography Updated successfully";
        }

        responseModel.Message = message;
        responseModel.StatusCode = 200;
        return Ok(new { responseModel });
    }

    private string UploadImages(List<IFormFile> Images)
    {
        string biographyImages;
        List<string> biographyImageList = new List<string>();
        string wwwRootPath = _hostEnvironment.WebRootPath;
        foreach (var file in Images)
        {
            if (file != null)
            {
                string fileName = file.FileName + Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"BiographyVideos");
                var extension = Path.GetExtension(file.FileName);

                using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }

                biographyImageList.Add(@"/BiographyVideos/" + fileName + extension);
            }
        }

        biographyImages = JsonSerializer.Serialize(biographyImageList);
        return biographyImages;
    }

    [HttpDelete]
    [Route("Admin/Dashboard/Biography/Delete/")]
    public IActionResult Delete(int? id)
    {
        ResponseModel responseModel = new ResponseModel();
        var obj = _unitOfWork.biography.GetFirstOrDefault(x => x.id == id);
        List<string> profileVideoUrlList = JsonSerializer.Deserialize<List<string>>(obj.profileVideoUrl);
        List<string> profileImageUrlList = JsonSerializer.Deserialize<List<string>>(obj.profileVideoUrl);

        if (obj == null)
        {
            responseModel.Message = "Error while deleting";
            responseModel.StatusCode = 400;
            var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return BadRequest(new { responseModel, Errors = errors2 });
        }

        deleteFromStorageFiles(profileImageUrlList);
        deleteFromStorageFiles(profileVideoUrlList);

        _unitOfWork.biography.Remove(obj);
        _unitOfWork.Save();
        
        responseModel.Message = "Biography was successfully removed";
        responseModel.StatusCode = 200;
        return Ok(responseModel);

    }

    [HttpDelete]
    [Route("Admin/Dashboard/Biography/Delete/Image")]
    public IActionResult DeleteAnImage(int? id, [FromForm] string imageToDeleteUrl )
    {
        ResponseModel responseModel = new ResponseModel();
        var obj = _unitOfWork.biography.GetFirstOrDefault(x => x.id == id);
        List<string> imageUrlList = JsonSerializer.Deserialize<List<string>>(obj.profileImageUrl);
        int counter = 0;
        Boolean ImageExist = false;
        if (obj == null)
        {
            responseModel.Message = "Error while deleting file";
            responseModel.StatusCode = 400;
            var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return BadRequest(new { responseModel, Errors = errors2 });
        }

        foreach (var imageUrl in imageUrlList)
        {
            if (imageUrl == imageToDeleteUrl)
            {
                imageUrlList.RemoveAt(counter);
                ImageExist = true;
                break;
            }

            counter++;
        }

        if (ImageExist)
        {
            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, imageToDeleteUrl.TrimStart('/'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
        }

        obj.profileImageUrl = JsonSerializer.Serialize(imageUrlList);
        _unitOfWork.biography.Update(obj);
        _unitOfWork.Save();

        responseModel.Message = "image in biography was successfully removed";
        responseModel.StatusCode = 200;
        return Ok(responseModel);
    }

    [HttpDelete]
    [Route("Admin/Dashboard/Biography/Delete/Video")]
    public IActionResult DeleteAVideo(int? id, [FromForm] string videoToDeleteUrl )
    {
        ResponseModel responseModel = new ResponseModel();
        var obj = _unitOfWork.biography.GetFirstOrDefault(x => x.id == id);
        List<string> VideoUrlList = JsonSerializer.Deserialize<List<string>>(obj.profileVideoUrl);
        int counter = 0;
        Boolean videoExist = false;
        if (obj == null)
        {
            responseModel.Message = "Error while deleting file";
            responseModel.StatusCode = 400;
            var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return BadRequest(new { responseModel, Errors = errors2 });
        }

        foreach (var vidoeUrl in VideoUrlList)
        {
            if (vidoeUrl == videoToDeleteUrl)
            {
                VideoUrlList.RemoveAt(counter);
                videoExist = true;
                break;
            }

            counter++;
        }

        if (videoExist)
        {
            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, videoToDeleteUrl.TrimStart('/'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
        }

        obj.profileVideoUrl = JsonSerializer.Serialize(VideoUrlList);
        _unitOfWork.biography.Update(obj);
        _unitOfWork.Save();

        responseModel.Message = "Video in biography was successfully removed";
        responseModel.StatusCode = 200;
        return Ok(responseModel);
    }
    private string UploadVideos(List<IFormFile> Videos)
    {
        string biographyVideos;
        List<string> biographyVideoList = new List<string>();
        string wwwRootPath = _hostEnvironment.WebRootPath;
        foreach (var file in Videos)
        {
            if (file != null)
            {
                string fileName = file.FileName + Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"BiographyVideos");
                var extension = Path.GetExtension(file.FileName);

                using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }

                biographyVideoList.Add(@"/BiographyVideos/" + fileName + extension);
            }
        }

        biographyVideos = JsonSerializer.Serialize(biographyVideoList);
        return biographyVideos;
    }
    
    private void deleteFromStorageFiles(List<string> files)
    {
        foreach (var file in files)
        {
            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, file.TrimStart('/'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
        }
    }
}
