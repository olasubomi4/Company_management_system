
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ObaGoupDataAccess.Repository.IRepository;
using ObaGroupModel;
using ObaGroupUtility;


namespace Oba_group2.Areas.Admin.Controllers;


[Area("Admin")]
[Authorize(Roles = Constants.Role_Admin)]
public class DocumentController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _hostEnvironment;
    
    public DocumentController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment )
    {
        _unitOfWork = unitOfWork;
        _hostEnvironment = hostEnvironment;
    }
    // GET
    public IActionResult Index()
    {
       IEnumerable<Document> documents= _unitOfWork.document.GetAll();
       return View(documents);
    }
    
    
    public IActionResult Upsert(int? id)
    {
        Document document = new Document();
        Documents documents = new Documents();
        if (id == null || id == 0)
        {
            documents.Document = document;
            return View(documents);
        }
        else
        {
            document = _unitOfWork.document.GetFirstOrDefault(u => u.Id == id);
            documents.Document = document;
            return View(documents);
        }
    }
    
    
     
    [HttpPost]
    [ValidateAntiForgeryToken]
    [RequestFormLimits(MultipartBodyLengthLimit = 6104857600)]
    [RequestSizeLimit(6104857600)]
    public IActionResult Upsert(Documents documents,IFormFileCollection d )
    {
        Document obj = documents.Document;
        List<string> documentsUrl = new List<string>();

        if (!ModelState.IsValid)
        {
            return Redirect("index");
        }

        string type = obj.Type;
       
        var files = documents.Files;
        string wwwRootPath = _hostEnvironment.WebRootPath;
        foreach (var file in files)
        {
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"documents");
                var extension = Path.GetExtension(file.FileName);

                if (obj.DocumentUrl != null)
                {
                    var oldImagePath = Path.Combine(wwwRootPath, obj.DocumentUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }
                documentsUrl.Add( @"/documents/" + fileName + extension);
                type=getDocumentType(file, type);
            }
        }

        obj.Type = type;
        obj.DocumentUrl = JsonSerializer.Serialize(documentsUrl);

        if (obj.Id == 0)
        {
            _unitOfWork.document.Add(obj);
        }
        else
        {
            _unitOfWork.document.Update(obj);
        }
    
        _unitOfWork.Save();
        if (obj.Id == 0)
        {
            TempData["success"] = "Document created successfully";
        }
        else
        {
            TempData["success"] = "Document Updated successfully";
        }

        return RedirectToAction("Index");
    }
   /* [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Category obj)
    {
        if (obj.Name == obj.DisplayOrder.ToString())
        {
            ModelState.AddModelError("Name","The Display order cannot be the same as the Name");
        }
        if (!ModelState.IsValid)
        {
            return View(obj);
        }

        _unitOfWork.Category.Add(obj);
        _unitOfWork.Save();
        TempData["success"] = "Category created successfully";
        return RedirectToAction("Index");
    }*/
   
   
   [HttpGet]
   public IActionResult GetAll()
   {
       var document = _unitOfWork.document.GetAll();
       return Json(new { data = document });
   }
   
   [HttpDelete]
   public IActionResult Delete(int? id)
   {
       var obj = _unitOfWork.document.GetFirstOrDefault(x=>x.Id==id);
       if (obj == null)
       {
           return Json(new { success = false, Message = "Error while deleting" });
       }
       var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.DocumentUrl.TrimStart('/'));
       if (System.IO.File.Exists(oldImagePath))
       {
           System.IO.File.Delete(oldImagePath);
       }
       _unitOfWork.document.Remove(obj);
       _unitOfWork.Save();
       return Json(new { success = true, Message = "Delete successful" });
        
   }

   private string getDocumentType(IFormFile file,string types)
   {
       var extension = Path.GetExtension(file.FileName);
       StringBuilder stringBuilder = new StringBuilder();
       if (types != null)
       {
           if (types.Contains(extension))
           {
               return types;
           }
           else
           {
               stringBuilder.Append(types);
               stringBuilder.Append(extension);
               return stringBuilder.ToString();
           }
       }

       return extension;
   }
   

}