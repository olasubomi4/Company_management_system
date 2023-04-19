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
    // GET
   /* public IActionResult Index()
    {
        return View();
    }
    */
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
   [ValidateAntiForgeryToken]
   public async Task<IActionResult> Login([FromForm] LoginModel Input )
    {
      //  returnUrl ??= Url.Content("~/");

    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
    ResponseModel responseModel = new ResponseModel();

        if (ModelState.IsValid)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
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
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                            protocol: Request.Scheme);

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
                            
                            responseModel.Message = "User created a new account with password.";
                            responseModel.StatusCode=200;
                            return Ok(responseModel);
                        //}
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                return BadRequest(new {responseModel, Errors =errors2 });
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
