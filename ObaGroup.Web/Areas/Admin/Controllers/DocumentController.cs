using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ObaGoupDataAccess.Repository.IRepository;
using ObaGroupModel;
using ObaGroupUtility;

namespace ObaGWebroup.Controllers;

[Area("Admin")]
//[Authorize(Roles = Constants.Role_Admin)]
public class DocumentController : Controller
{
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobUploader _blobUploader;

    public DocumentController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment, IBlobUploader blobUploader)
    {
        _unitOfWork = unitOfWork;
        _hostEnvironment = hostEnvironment;
        _blobUploader = blobUploader;
    }
    // GET

    [HttpGet]
    [Route("dashboard/documents")]
    public IActionResult Doc()
    {
        var documents = _unitOfWork.document.GetAll();

        return File("~/dashboard/documents/index.html", "text/html");
    }


    public IActionResult Upsert(int? id)
    {
        var document = new Document();
        var documents = new Documents();
        if (id == null || id == 0)
        {
            documents.Document = document;
            return View(documents);
        }

        document = _unitOfWork.document.GetFirstOrDefault(u => u.Id == id);
        documents.Document = document;
        return View(documents);
    }


    //[ValidateAntiForgeryToken]
    [HttpGet]
    [Route(Constants.Get_A_Document_Endpoint)]
    public IActionResult GetById(int? id)
    {
        var responseModel = new ResponseModel();
        if (id == null)
        {
            responseModel.Message = "Missing id field";
            responseModel.StatusCode = 400;

            ModelState.AddModelError(string.Empty, "The document id should be provided");
            var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);

            return BadRequest(new { responseModel, Errors = errors2 });
        }

        var document = _unitOfWork.document.GetFirstOrDefault(x => x.Id == id);
        return Ok(document);
    }

    [HttpPost]
    [RequestFormLimits(MultipartBodyLengthLimit = 6104857600)]
    [RequestSizeLimit(6104857600)]
    [Route(Constants.Upsert_A_Document_Endpoint)]
    public IActionResult Upsert([FromForm] Documents documents)
    {
        var isCreate = false;
        string message;
        var obj = new Document();
        var documentsUrl = new List<string>();
        var responseModel = new ResponseModel();

        if (!ModelState.IsValid)
        {
            responseModel.Message = "Bad Request";
            responseModel.StatusCode = 400;
            var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return BadRequest(new { responseModel, Errors = errors2 });
        }

        obj = documents.Document;
        var type = obj.Type;
        var files = documents.Files;
        var wwwRootPath = _hostEnvironment.WebRootPath;
        foreach (var file in files)
            if (file != null)
            {
                // var uploads = Path.Combine(wwwRootPath, @"documents");
                // if (obj.DocumentUrl != null)
                // {
                //     var oldImagePath = Path.Combine(wwwRootPath, obj.DocumentUrl.TrimStart('/'));
                //     if (System.IO.File.Exists(oldImagePath))
                //     {
                //         System.IO.File.Delete(oldImagePath);
                //     }
                // }
                //
                // using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                // {
                //     file.CopyTo(fileStreams);
                // }
                var fileName = file.FileName + Guid.NewGuid();
                var documentUri = _blobUploader.UploadDocument(file, fileName);
                documentsUrl.Add(documentUri);
                type = getDocumentType(file, type);
            }

        obj.Type = type;


        if (obj.Id == 0)
        {
            obj.DocumentUrl = JsonSerializer.Serialize(documentsUrl);
            _unitOfWork.document.Add(obj);
            isCreate = true;
        }
        else
        {
            obj.DocumentUrl = obj.DocumentUrl + JsonSerializer.Serialize(documentsUrl);
            _unitOfWork.document.Update(obj);
        }

        _unitOfWork.Save();
        if (isCreate)
            message = "Document created successfully";
        else
            message = "Document Updated successfully";

        responseModel.Message = message;
        responseModel.StatusCode = 200;
        return Ok(new { responseModel });
    }


    [HttpGet]
    [Route(Constants.List_Documents_Endpoint)]
    public IActionResult GetAll()
    {
        var document = _unitOfWork.document.GetAll();
        return Ok(document);
    }

    [HttpDelete]
    [Route(Constants.Delete_A_Document_Endpoint)]
    public IActionResult Delete(int? id)
    {
        var responseModel = new ResponseModel();
        if (id == null)
        {
            responseModel.Message = "Missing id field";
            responseModel.StatusCode = 400;

            ModelState.AddModelError(string.Empty, "The document id should be provided");
            var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);

            return BadRequest(new { responseModel, Errors = errors2 });
        }

        var obj = _unitOfWork.document.GetFirstOrDefault(x => x.Id == id);
        var documentUrlList = JsonSerializer.Deserialize<List<string>>(obj.DocumentUrl);
        if (obj == null)
        {
            responseModel.Message = "Error while deleting";
            responseModel.StatusCode = 400;
            var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return BadRequest(new { responseModel, Errors = errors2 });
        }

        foreach (var documentUrl in documentUrlList)
        {
            var fileName = new Uri(documentUrl).Segments[2];

            _blobUploader.DeleteDocument(fileName);
            // var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, documentUrl.TrimStart('/'));
            // if (System.IO.File.Exists(oldImagePath))
            // {
            //     System.IO.File.Delete(oldImagePath);
            // }
        }

        _unitOfWork.document.Remove(obj);
        _unitOfWork.Save();
        responseModel.Message = "Document was successfully removed";
        responseModel.StatusCode = 200;
        return Ok(responseModel);
    }

    [HttpDelete]
    [Route(Constants.Delete_A_Document_File_Endpoint)]
    public IActionResult DeleteFile(int? id, [FromForm] string fileUrl)
    {
        var responseModel = new ResponseModel();
        // fileUrl = fileUrl.Replace("\\", "");
        if (id == null)
        {
            responseModel.Message = "Missing id field";
            responseModel.StatusCode = 400;

            ModelState.AddModelError(string.Empty, "The document id should be provided");
            var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);

            return BadRequest(new { responseModel, Errors = errors2 });
        }

        var obj = _unitOfWork.document.GetFirstOrDefault(x => x.Id == id);
        var documentUrlList = JsonSerializer.Deserialize<List<string>>(obj.DocumentUrl);
        var counter = 0;
        var documentExist = false;
        if (obj == null)
        {
            responseModel.Message = "Error while deleting file";
            responseModel.StatusCode = 400;
            var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return BadRequest(new { responseModel, Errors = errors2 });
        }

        foreach (var documentUrl in documentUrlList)
        {
            if (documentUrl == fileUrl)
            {
                documentUrlList.RemoveAt(counter);
                documentExist = true;
                break;
            }

            counter++;
        }

        if (counter >= documentUrlList.Count || counter == 0)
        {
            responseModel.Message = "File does not exist";
            responseModel.StatusCode = 400;

            ModelState.AddModelError(string.Empty, "The file you are trying to delete does not exist");
            var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return BadRequest(new { responseModel, Errors = errors2 });
        }

        if (documentExist)
        {
            var fileName = new Uri(fileUrl).Segments[2];

            _blobUploader.DeleteDocument(fileName);

            // var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, fileUrl.TrimStart('/'));
            // if (System.IO.File.Exists(oldImagePath))
            // {
            //     System.IO.File.Delete(oldImagePath);
            // }
        }

        obj.DocumentUrl = JsonSerializer.Serialize(documentUrlList);
        _unitOfWork.document.Update(obj);
        _unitOfWork.Save();

        responseModel.Message = "File in document was successfully removed";
        responseModel.StatusCode = 200;
        return Ok(responseModel);
    }

    private string getDocumentType(IFormFile file, string types)
    {
        var extension = Path.GetExtension(file.FileName);
        var stringBuilder = new StringBuilder();
        if (types != null)
        {
            if (types.Contains(extension)) return types;

            stringBuilder.Append(types);
            stringBuilder.Append(extension);
            return stringBuilder.ToString();
        }

        return extension;
    }
}