using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Oba_group2.Areas.Identity.Pages.Account;
using ObaGoupDataAccess.Repository.IRepository;
using ObaGroupModel;
using ObaGroupUtility;
using LoginModel = ObaGroupModel.LoginModel;
using RegisterModel = ObaGroupModel.RegisterModel;

namespace Oba_group2.Areas.Admin.Controllers;

[Area("Admin")]
public class AccountController : Controller
{
 
       private readonly SignInManager<IdentityUser> _signInManager;
       private readonly ILogger<LoginModel> _logger;
       private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger2;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

   public AccountController(SignInManager<IdentityUser> signInManager, ILogger<LoginModel> logger,  UserManager<IdentityUser> userManager,
       IUserStore<IdentityUser> userStore,
       ILogger<RegisterModel> logger2,
       IEmailSender emailSender,
       RoleManager<IdentityRole> roleManager,
       IUnitOfWork unitOfWork)
   {
       _signInManager = signInManager;
       _logger = logger;
       _userManager = userManager;
       _userStore = userStore;
       _emailStore = GetEmailStore();
       _logger2 = logger2;
       _emailSender = emailSender;
       _roleManager = roleManager;
       _unitOfWork = unitOfWork;
   }
   public IList<AuthenticationScheme> ExternalLogins { get; set; }
   
   
   
   [HttpPost]
   public async Task<IActionResult> Login([FromForm] LoginModel Input )
    {
      //  returnUrl ??= Url.Content("~/");

    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
    ResponseModel responseModel = new ResponseModel();

        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                responseModel.Message = "User logged in.";
                responseModel.StatusCode=200;
                return Ok(responseModel);
            }
            
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            responseModel.StatusCode = 400;
            responseModel.Message = "Invalid login attempt.";
            
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return BadRequest(new {responseModel, Errors =errors});
        }
        responseModel.Message = "Bad Request";
        responseModel.StatusCode = 400;
        var errors2 = ModelState.Values.SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage);
        return BadRequest(new {responseModel, Errors =errors2 });
    }
   
        [HttpPost]
        [ValidateAntiForgeryToken]
      public async Task<IActionResult> CreateUser([FromForm] RegisterModel Input)
      {
          ResponseModel responseModel = new ResponseModel();
               string returnUrl = Url.Content("~/");
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                if (ModelState.IsValid)
                {
                    var user = CreateUser();

                    await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                    await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                    user.FirstName = Input.FirstName;
                    user.LastName = Input.LastName;
                    user.Address = Input.Address;
                    user.Position = Input.Position;
                    
                    
                 
                    var result = await _userManager.CreateAsync(user, Input.Password);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created a new account with password.");

                        if (Input.Role == null)
                        {
                            await _userManager.AddToRoleAsync(user, Constants.Role_Staff);
                        }
                        else
                        {
                            await _userManager.AddToRoleAsync(user, Input.Role);
                        }
                        var userId = await _userManager.GetUserIdAsync(user);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl =$"{Request.Scheme}://{Request.Host}/Admin/Account/ConfirmEmail?userId={userId}&code={code}&returnUrl={returnUrl}";
                        

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        /*if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("RegisterConfirmation",
                                new { email = Input.Email, returnUrl = returnUrl });
                        }
                        else
                        {*/
                            //await _signInManager.SignInAsync(user, isPersistent: false);
                            
                            responseModel.Message = "User created a new account with password, please confirm your account by click on the link sent to your email ";
                            responseModel.StatusCode=200;
                            return Ok(responseModel);
                        //}
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                responseModel.Message = "Bad Request";
                responseModel.StatusCode = 400;
                var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                return BadRequest(new {responseModel, Errors =errors2 });
            }
         
      [HttpPost]
      public async Task<IActionResult> ConfirmEmail(string userId, string code)
      {
          ConfirmEmail confirmEmail = new ConfirmEmail();
          ResponseModel responseModel = new ResponseModel();
          if (userId == null || code == null)
          {
              responseModel.Message = "User does not exist";
              responseModel.StatusCode = 404;
              return NotFound(responseModel);
          }

          var user = await _userManager.FindByIdAsync(userId);
          if (user == null)
          {
              responseModel.Message = "User does not exist";
              responseModel.StatusCode = 404;
              return NotFound(responseModel);
          }

          code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
          var result = await _userManager.ConfirmEmailAsync(user, code);
          confirmEmail.StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
          if (result.Succeeded)
          {
              responseModel.StatusCode = 200;
              responseModel.Message = confirmEmail.StatusMessage;
              return Ok(responseModel);
          }
          else
          {
              responseModel.StatusCode = 400;
              responseModel.Message = confirmEmail.StatusMessage;
              foreach (var error in result.Errors)
              {
                  ModelState.AddModelError(string.Empty, error.Description);
              }
              var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                  .Select(e => e.ErrorMessage);
              return BadRequest(new {responseModel, Errors =errors2 });
          }
      }
      
      public async Task<IActionResult> ForgotPassword([FromForm] ForgotPassword Input)
      {
          ResponseModel responseModel = new ResponseModel();
          if (ModelState.IsValid)
          {
              var user = await _userManager.FindByEmailAsync(Input.Email);
              if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
              {
                  responseModel.StatusCode = 200;
                  responseModel.Message="If an account with the confirmed email address you provided exists in our system," +
                                        " we have sent you an email with instructions on how to reset your password." +
                                        " Please check your email and follow the instructions provided. " +
                                        "If you don't receive the email within a few minutes, " +
                                        "please check your spam folder or try again with a different email address.";
                  return Ok(responseModel);
              }
              
              var code = await _userManager.GeneratePasswordResetTokenAsync(user);
              code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
             
              var callbackUrl =  $"{Request.Scheme}://{Request.Host}/Admin/Account/ResetPassword?code={code}";

              await _emailSender.SendEmailAsync(
                  Input.Email,
                  "Reset Password",
                  $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

              responseModel.StatusCode = 200;
              responseModel.Message = "Please reset your password by clicking on the link sent to your email";
              return Ok(responseModel);
          }
          responseModel.Message = "Bad Request";
          responseModel.StatusCode = 400;
          var errors2 = ModelState.Values.SelectMany(v => v.Errors)
              .Select(e => e.ErrorMessage);
          return BadRequest(new {responseModel, Errors =errors2 });
      }
      public async Task<IActionResult> ResetPassword([FromForm] ResetPassword Input)
      {
          ResponseModel responseModel = new ResponseModel();
          if (!ModelState.IsValid)
          {
              responseModel.Message = "Bad Request";
              responseModel.StatusCode = 400;
              var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                  .Select(e => e.ErrorMessage);
              return BadRequest(new {responseModel, Errors =errors2 });
          }

          Input.Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Input.Code));
          var user = await _userManager.FindByEmailAsync(Input.Email);
          if (user == null)
          {
              // Don't reveal that the user does not exist
              responseModel.StatusCode = 404;
              responseModel.Message = "User does not exist.";
              return NotFound(responseModel);
          }

          var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
          if (result.Succeeded)
          {
              responseModel.StatusCode = 200;
              responseModel.Message = "Your password has been reset.";
              return Ok(responseModel);
          }
          responseModel.Message = "Bad Request";
          responseModel.StatusCode = 400;
          
          foreach (var error in result.Errors)
          {
              ModelState.AddModelError(string.Empty, error.Description);
          }
          var errors = ModelState.Values.SelectMany(v => v.Errors)
              .Select(e => e.ErrorMessage);
          return BadRequest(new {responseModel, Errors =errors });
      }
      
      public async Task<IActionResult> ResendEmailVerification([FromForm] EmailVerification Input)
      {
          ResponseModel responseModel = new ResponseModel();
          if (!ModelState.IsValid)
          {
              responseModel.StatusCode = 400;
              responseModel.Message = "Bad request";
              var errors = ModelState.Values.SelectMany(v => v.Errors)
                  .Select(e => e.ErrorMessage);
              return BadRequest(new {responseModel, Errors =errors });
          }

          var user = await _userManager.FindByEmailAsync(Input.Email);
          if (user == null)
          {
              ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
              responseModel.StatusCode = 200;
              responseModel.Message = "Verification email sent. Please check your email.";
              return Ok(responseModel);
          }

          var userId = await _userManager.GetUserIdAsync(user);
          var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
          code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
          var callbackUrl =$"{Request.Scheme}://{Request.Host}/Admin/Account/ConfirmEmail?userId={userId}&code={code}";
          
          await _emailSender.SendEmailAsync(
              Input.Email,
              "Confirm your email",
              $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

          ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
          responseModel.StatusCode = 200;
          responseModel.Message = "Verification email sent. Please check your email.";
          return Ok(responseModel);
      }
      
      private ApplicationUser CreateUser()
   {
       try
       {
           return Activator.CreateInstance<ApplicationUser>();
       }
       catch
       {
           throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                                               $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                                               $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
       }
   }

   private IUserEmailStore<IdentityUser> GetEmailStore()
   {
       if (!_userManager.SupportsUserEmail)
       {
           throw new NotSupportedException("The default UI requires a user store with email support.");
       }

       return (IUserEmailStore<IdentityUser>)_userStore;
   }
   
}
