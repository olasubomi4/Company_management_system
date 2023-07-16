using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObaGoupDataAccess.Repository.IRepository;
using ObaGroupModel;
using ObaGroupModel.Validation;
using ObaGroupUtility;
using System.Text.Json;

namespace Oba_group2.Areas.Admin.Controllers;

[Area("Admin")]
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
    [Route("/dashboard/biographys")]
    public IActionResult Index()
    {
        return File("~/dashboard/biography/index.html", "text/html");
    }

    [HttpGet]
    [Route(Constants.Get_A_Biography_Endpoint)]
    [Authorize(Roles = Constants.Role_Admin+","+Constants.Role_Staff)]
    public IActionResult GetById(int? id)
    {
        ResponseModel responseModel = new ResponseModel();
        if (id == null)
        {
            responseModel.Message = "Missing id field";
            responseModel.StatusCode = 400;
           
            ModelState.AddModelError(string.Empty, "The biography id should be provided");
            var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);

            return BadRequest(new {responseModel, Errors =errors2 });
        }
        Biography biography = _unitOfWork.biography.GetFirstOrDefault(x=> x.Id==id);
        return Ok(biography);
    }

   
    [HttpGet]
    [Route(Constants.List_All_Biographies_Endpoint)]
    //[Authorize(Roles = Constants.Role_Admin+","+Constants.Role_Staff)]
    public IActionResult GetAll()
    {
        IEnumerable<Biography> biographies = _unitOfWork.biography.GetAll();
        return Ok(biographies);
    }

    [HttpPost]
    [RequestFormLimits(MultipartBodyLengthLimit = 6104857600)]
    [RequestSizeLimit(6104857600)]
   // [Authorize(Roles = Constants.Role_Admin)]
    [Route(Constants.Upsert_Biography_Endpoint)]
    public IActionResult Upsert([FromForm] Biographies biographies)
    {
        Boolean isCreate = false;
        string message;
        Biography obj = new Biography();
        ResponseModel responseModel = new ResponseModel();


        if (!ModelState.IsValid)
        {
            responseModel.Message = "Bad Request";
            responseModel.StatusCode = 400;
            var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return BadRequest(new { responseModel, Errors = errors2 });
        }
        obj = biographies.Biography;
        if (obj.Id == 0)
        {
            if(biographies.ProfileImage!=null)
            {
                obj.profileImageUrl = UploadImages(biographies.ProfileImage);
            }

            if (biographies.ProfileVideo != null)
            {
                obj.profileVideoUrl = UploadVideos(biographies.ProfileVideo);
            }

            _unitOfWork.biography.Add(obj);
            isCreate = true;
        }
        else
        {
            if (biographies.ProfileImage != null)
            {
                obj.profileImageUrl = obj.profileImageUrl + UploadImages(biographies.ProfileImage);
            }

            if (biographies.ProfileVideo != null)
            {
                obj.profileVideoUrl = obj.profileVideoUrl + UploadVideos(biographies.ProfileVideo);
            }

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
                var uploads = Path.Combine(wwwRootPath, @"biographyImages");
                var extension = Path.GetExtension(file.FileName);

                using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }

                biographyImageList.Add(@"/biographyImages/" + fileName + extension);
            }
        }

        biographyImages = JsonSerializer.Serialize(biographyImageList);
        return biographyImages;
    }

    [HttpDelete]
    [Route(Constants.Delete_A_Biography_Endpoint)]
    [Authorize(Roles = Constants.Role_Admin)]
    public IActionResult Delete(int? id)
    {
        ResponseModel responseModel = new ResponseModel();
        if (id == null)
        {
            responseModel.Message = "Missing id field";
            responseModel.StatusCode = 400;
           
            ModelState.AddModelError(string.Empty, "The biography id should be provided");
            var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);

            return BadRequest(new {responseModel, Errors =errors2 });
        }
        var obj = _unitOfWork.biography.GetFirstOrDefault(x => x.Id == id);
        List<string> profileVideoUrlList = JsonSerializer.Deserialize<List<string>>(obj.profileVideoUrl);
        List<string> profileImageUrlList = JsonSerializer.Deserialize<List<string>>(obj.profileImageUrl);

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
    [Route(Constants.Delete_A_Biography_Image_Endpoint)]
    [Authorize(Roles = Constants.Role_Admin)]
    public IActionResult DeleteAnImage(int? id, [FromForm] string imageToDeleteUrl )
    {
        ResponseModel responseModel = new ResponseModel();
        imageToDeleteUrl = imageToDeleteUrl.Replace("\\", "");
        if (id == null)
        {
            responseModel.Message = "Missing id field";
            responseModel.StatusCode = 400;
           
            ModelState.AddModelError(string.Empty, "The biography id should be provided");
            var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);

            return BadRequest(new {responseModel, Errors =errors2 });
        }
        var obj = _unitOfWork.biography.GetFirstOrDefault(x => x.Id == id);
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

        if (counter >= imageUrlList.Count || counter==0)
        {
            responseModel.Message = "Image does not exist";
            responseModel.StatusCode = 400;
           
            ModelState.AddModelError(string.Empty, "The image you are trying to delete does not exist");
            var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return BadRequest(new {responseModel, Errors =errors2 });
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
    [Route(Constants.Delete_A_BioGraphy_Video_Endpoint)]
    [Authorize(Roles = Constants.Role_Admin)]
    public IActionResult DeleteAVideo(int? id, [FromForm] string videoToDeleteUrl )
    {
        ResponseModel responseModel = new ResponseModel();
        videoToDeleteUrl = videoToDeleteUrl.Replace("\\", "");
        if (id == null)
        {
            responseModel.Message = "Missing id field";
            responseModel.StatusCode = 400;
           
            ModelState.AddModelError(string.Empty, "The biography id should be provided");
            var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);

            return BadRequest(new {responseModel, Errors =errors2 });
        }
        var obj = _unitOfWork.biography.GetFirstOrDefault(x => x.Id == id);
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
        if (counter >= VideoUrlList.Count || counter==0)
        {
            responseModel.Message = "Video does not exist";
            responseModel.StatusCode = 400;
           
            ModelState.AddModelError(string.Empty, "The video you are trying to delete does not exist");
            var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return BadRequest(new {responseModel, Errors =errors2 });
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
                var uploads = Path.Combine(wwwRootPath, @"biographyVideos");
                var extension = Path.GetExtension(file.FileName);

                using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }

                biographyVideoList.Add(@"/biographyVideos/" + fileName + extension);
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
