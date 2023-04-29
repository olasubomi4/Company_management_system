using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ObaGoupDataAccess.Repository.IRepository;
using ObaGroupModel;

namespace Oba_group2.Areas.Admin.Controllers;

[Area("Admin")]
public class ManageAccountController : Controller
{
    // GET
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<LoginModel> _logger;
    private readonly IWebHostEnvironment _hostEnvironment;

    public ManageAccountController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IUnitOfWork unitOfWork,ILogger<LoginModel> logger,
        IWebHostEnvironment hostEnvironment)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _unitOfWork = unitOfWork;
        ILogger<RegisterModel> logger2;
        _hostEnvironment = hostEnvironment;
    }
    
    private async Task LoadAsync(IdentityUser user)
    {
        var userName = await _userManager.GetUserNameAsync(user);
        var phoneNumber = _unitOfWork.ApplicationUser.GetFirstOrDefault(u=>u.Id==user.Id).PhoneNumber;
        var firstName = _unitOfWork.ApplicationUser.GetFirstOrDefault(u=>u.Id==user.Id).FirstName;
        var lastName = _unitOfWork.ApplicationUser.GetFirstOrDefault(u=>u.Id==user.Id).LastName;
        var address = _unitOfWork.ApplicationUser.GetFirstOrDefault(u=>u.Id==user.Id).Address;
        var position = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == user.Id).Position;



        UserInformation userInformation = new UserInformation();

        userInformation.Username = userName;
        userInformation.PhoneNumber = phoneNumber;
        userInformation.FirstName = firstName;
        userInformation.LastName = lastName;
        userInformation.Address = address;
        userInformation.Position = position;
    
    }
    
    [Route("Admin/Dashboard/UpdateUserInformation")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateUserInformation([FromForm] ApplicationUser Input, [FromForm] ImageFIleForm imageFile )
    {
        ResponseModel responseModel = new ResponseModel();
        var user = await _userManager.GetUserAsync(User);
        ApplicationUser applicationUser = new ApplicationUser();
        string imageUrl = null;
        if (user == null)
        {
            responseModel.Message = "Unable to load user";
            responseModel.StatusCode = 404;
            var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return NotFound(responseModel);
        }

        if (!ModelState.IsValid)
        {
            responseModel.Message = "Bad Request, failed to update user information";
            responseModel.StatusCode = 400;
            var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return BadRequest(new {responseModel, Errors =errors2 });
        }

        applicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == user.Id);
        imageUrl = UploadImage(imageFile,applicationUser.ImageUrl);
          
        if (imageUrl == null)
        {
            responseModel.Message = "Bad Request";
            responseModel.StatusCode = 400;
              
            ModelState.AddModelError(string.Empty, "Unable to upload profile image, try another image");
              
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return BadRequest(new {responseModel, Errors =errors });
  
        }
        
       
        var phoneNumber = _unitOfWork.ApplicationUser.GetFirstOrDefault(u=>u.Id==user.Id).PhoneNumber;
        if (Input.PhoneNumber != phoneNumber)
        {
            var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            if (!setPhoneResult.Succeeded)
            {
                responseModel.Message = "Unexpected error when trying to set phone number.";
                responseModel.StatusCode = 400;
                foreach (var error in setPhoneResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                return BadRequest(new {responseModel, Errors =errors2});
            }
        }

        await _signInManager.RefreshSignInAsync(user);
        applicationUser.Id = user.Id;
        applicationUser.FirstName = Input.FirstName;
        applicationUser.LastName = Input.LastName;
        applicationUser.Address = Input.Address;
        applicationUser.Position = Input.Position;
        applicationUser.ImageUrl = imageUrl;
        _unitOfWork.ApplicationUser.Update(applicationUser);
        _unitOfWork.Save();
        
        responseModel.Message = "Your profile has been updated";
        responseModel.StatusCode = 200;
        return Ok(responseModel);

    }
    
    [Route("Admin/Dashboard/ChangePassword")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword([FromForm] ChangePassword Input)
    {
        ResponseModel responseModel = new ResponseModel();
        if (!ModelState.IsValid)
        {
            responseModel.Message = "Bad Request";
            responseModel.StatusCode = 400;
            var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return BadRequest(new {responseModel, Errors =errors2});
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            responseModel.Message = "Unable to load user";
            responseModel.StatusCode = 404;
            var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return NotFound(responseModel);
        }

        var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
        if (!changePasswordResult.Succeeded)
        {
           
            responseModel.Message = "Unable to Change user";
            responseModel.StatusCode = 400;
            foreach (var error in changePasswordResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return BadRequest(new {responseModel, Errors =errors2 });
        }

        await _signInManager.RefreshSignInAsync(user);
     //   _logger.LogInformation("User changed their password successfully.");

        responseModel.Message = "Your password has been changed.";
        responseModel.StatusCode = 200;
        return Ok(responseModel);
    }
    
    private string UploadImage(ImageFIleForm imageFile, string ImageUrl)
    {
        string wwwRootPath = _hostEnvironment.WebRootPath;

        if (imageFile != null)
        {
            string fileName = imageFile.Image.FileName + Guid.NewGuid().ToString();
            var uploads = Path.Combine(wwwRootPath, @"profiles");
            var extension = Path.GetExtension(imageFile.Image.FileName);

            if (ImageUrl != null)
            {
                var oldImagePath = Path.Combine(wwwRootPath, ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
            {
                imageFile.Image.CopyTo(fileStreams);
            }

            ImageUrl = (@"/profiles/" + fileName + extension);
            return ImageUrl;
        }

        return null;
    }

}

