using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using ObaGoupDataAccess.Repository.IRepository;
using ObaGroupModel;
using ObaGroupUtility;

namespace ObaGWebroup.Controllers;
[Area("Admin")]
public class AccountController : Controller
{
    private readonly IBlobUploader _blobUploader;
    private readonly IEmailSender _emailSender;
    private readonly IUserEmailStore<IdentityUser> _emailStore;
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly IKeyVaultManager _keyVaultManager;
    private readonly ILogger<LoginModel> _logger;
    private readonly ILogger<RegisterModel> _logger2;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserStore<IdentityUser> _userStore;

    public AccountController(SignInManager<IdentityUser> signInManager, ILogger<LoginModel> logger,
        UserManager<IdentityUser> userManager,
        IUserStore<IdentityUser> userStore,
        ILogger<RegisterModel> logger2,
        IEmailSender emailSender,
        RoleManager<IdentityRole> roleManager,
        IUnitOfWork unitOfWork,
        IWebHostEnvironment hostEnvironment,
        IKeyVaultManager keyVaultManager, IBlobUploader blobUploader)
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
        _hostEnvironment = hostEnvironment;
        _keyVaultManager = keyVaultManager;
        _blobUploader = blobUploader;
    }

    [BindProperty] public InputModel Input { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string ProviderDisplayName { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string ReturnUrl { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [TempData]
    public string ErrorMessage { get; set; }

    public IList<AuthenticationScheme> ExternalLogins { get; set; }

    [HttpGet]
    [Route(Constants.Login_Endpoint)]
    public ActionResult Login()
    {
        var redirectUrl = $"{Request.Scheme}://{Request.Host}{Constants.UploadPage}";

        _logger.LogInformation("redirect url " + redirectUrl);
        if (User.Identity.IsAuthenticated) return Redirect(redirectUrl);

        _logger.LogInformation("going to view");

        return View();
    }

    [HttpGet]
    [Route("/dashboard/profile")]
    public IActionResult Profile()
    {
        return File("~/dashboard/profile/index.html", "text/html");
    }

    [HttpGet]
    [Route("/dashboard/users")]
    [Authorize(Roles = Constants.Role_Admin)]
    public IActionResult Users()
    {
        return File("~/dashboard/users/index.html", "text/html");
    }

    [HttpPost]
    [Route(Constants.Login_Endpoint)]
    public async Task<IActionResult> Login(string provider = "Google")
    {
        // string provider = "Google";
        var redirectUrl = $"{Request.Scheme}://{Request.Host}{Constants.Login_Google_Callback_Endpoint}";
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return new ChallengeResult(provider, properties);
    }

    [HttpGet]
    [Route(Constants.Login_Google_Callback_Endpoint)]
    public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
    {
        var responseModel = new ResponseModel();
        returnUrl = returnUrl ?? Url.Content("~/");
        if (remoteError != null)
        {
            ErrorMessage = $"Error from external provider: {remoteError}";
            _logger.LogInformation(ErrorMessage);
            responseModel.Message = ErrorMessage;
            responseModel.StatusCode = 401;
            return Unauthorized(responseModel);
        }

        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            ErrorMessage = "Error loading external login information.";
            _logger.LogInformation(ErrorMessage);
            responseModel.Message = ErrorMessage;
            responseModel.StatusCode = 401;
            return Unauthorized(responseModel);
        }

        // Sign in the user with this external login provider if the user already has a login.
        var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, true);
        if (result.Succeeded)
        {
            var input = new InputModel();
            input.Email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var applicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Email == input.Email);

            _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name,
                info.LoginProvider);
            var roles = await _userManager.GetRolesAsync(applicationUser);
            var role = roles[0];
            _logger.LogInformation("User logged in.");
            responseModel.Message = "User logged in.";
            responseModel.StatusCode = 200;
            SetCsrfToken(role);

            // return JsonBody(responseModel);
            //return Ok(responseModel);
            var redirectUrl = $"{Request.Scheme}://{Request.Host}{Constants.UploadPage}";
            return Redirect(redirectUrl);
        }

        if (result.IsLockedOut)
        {
            _logger.LogInformation("Account Locked");
            responseModel.Message = "Account Locked";
            responseModel.StatusCode = 401;
            return Unauthorized(responseModel);
        }

        // If the user does not have an account, then ask the user to create an account.
        ReturnUrl = returnUrl;
        ProviderDisplayName = info.ProviderDisplayName;
        if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
        {
            Input = new InputModel
            {
                Email = info.Principal.FindFirstValue(ClaimTypes.Email)
            };

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user != null)
            {
                var resultTemp = await _userManager.AddLoginAsync(user, info);
                if (resultTemp.Succeeded)
                {
                    await _signInManager.SignInAsync(user, true);

                    var applicationUser =
                        _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Email == Input.Email);

                    _logger.LogInformation("{Name} logged in with {LoginProvider} provider.",
                        info.Principal.Identity.Name, info.LoginProvider);
                    var roles = await _userManager.GetRolesAsync(applicationUser);
                    var role = roles[0];
                    _logger.LogInformation("User logged in.");
                    responseModel.Message = "User logged in.";
                    responseModel.StatusCode = 200;
                    SetCsrfToken(role);
                    var redirectUrl = $"{Request.Scheme}://{Request.Host}{Constants.UploadPage}";
                    return Redirect(redirectUrl);
                }
            }

            _logger.LogInformation("User does not have permission to this website");
            responseModel.Message = "User does not have permission to this website";
            responseModel.StatusCode = 401;
            return Unauthorized(responseModel);
        }


        _logger.LogInformation("User does not have permission to this website");
        responseModel.Message = "User does not have permission to this website";
        responseModel.StatusCode = 401;
        return Unauthorized(responseModel);
        _logger.LogInformation("User does not have permission to this website");
        responseModel.Message = "User does not have permission to this website";
        responseModel.StatusCode = 401;
        return Unauthorized(responseModel);
    }
    //string returnUrl;
    //returnUrl ??= Url.Content("~/");


    // ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
    // ResponseModel responseModel = new ResponseModel();
    //
    //     if (ModelState.IsValid)
    //     {
    //         var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
    //         if (result.Succeeded)
    //         {
    //             ApplicationUser applicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Email == Input.Email);
    //             var roles = await _userManager.GetRolesAsync(applicationUser);
    //             var role = roles[0];
    //           
    //             _logger.LogInformation("User logged in.");
    //             responseModel.Message = "User logged in.";
    //             responseModel.StatusCode=200;
    //             SetCsrfToken(role);
    //             return Ok(responseModel);
    //         }
    //         
    //         ModelState.AddModelError(string.Empty, "Invalid login attempt.");
    //         responseModel.StatusCode = 400;
    //         responseModel.Message = "Invalid login attempt.";
    //         
    //         var errors = ModelState.Values.SelectMany(v => v.Errors)
    //             .Select(e => e.ErrorMessage);
    //         return BadRequest(new {responseModel, Errors =errors});
    //     }
    //     responseModel.Message = "Bad Request";
    //     responseModel.StatusCode = 400;
    //     var errors2 = ModelState.Values.SelectMany(v => v.Errors)
    //         .Select(e => e.ErrorMessage);
    //     return BadRequest(new {responseModel, Errors =errors2 });
    // }

    [HttpPost]
    [Route(Constants.Create_User_EndPoint)]
    [Authorize(Roles = Constants.Role_Admin)]
    public async Task<IActionResult> CreateUser([FromForm] RegisterModel Input, [FromForm] string? returnUrl,
        [FromForm] ImageFIleForm image)
    {
        string imageUrl = null;
        var responseModel = new ResponseModel();
        if (!ModelState.IsValid)
        {
            responseModel.Message = "Bad Request";
            responseModel.StatusCode = 400;
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return BadRequest(new { responseModel, Errors = errors });
        }

        imageUrl = UploadImage(image);

        if (imageUrl == null)
        {
            responseModel.Message = "Bad Request";
            responseModel.StatusCode = 400;

            ModelState.AddModelError(string.Empty, "Unable to upload profile image, try another image");

            var errors = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return BadRequest(new { responseModel, Errors = errors });
        }

        returnUrl ??= Url.Content("~/");
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
            user.ImageUrl = imageUrl;

            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                // _logger.LogInformation("User created a new account with password.");

                if (Input.Role == null)
                    await _userManager.AddToRoleAsync(user, Constants.Role_Staff);
                else
                    await _userManager.AddToRoleAsync(user, Input.Role);
                // var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                // code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = $"{Request.Scheme}://{Request.Host}{Constants.Login_Endpoint}";
                await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    $" <tr>\r\n                      <td align=\"left\" width=\"30%\" style=\"vertical-align: top\">\r\n                        <p\r\n                          align=\"\"\r\n                          width=\"50%\"\r\n                          style=\"\r\n                            font-size: 20px;\r\n                            color: #2579a9;\r\n                            font-weight: 700;\r\n                          \"\r\n                        >\r\n                          Hi {user.FirstName}\r\n                        </p>\r\n                        <p\r\n                          style=\"\r\n                            color: #333;\r\n                            font-weight: 900;\r\n                            font-size: 16px;\r\n                            line-height: 24px;\r\n                          \"\r\n                        >\r\n                          Welcome to OBA GROUP, Lorem ipsum dolor sit amet\r\n                          consectetur.\r\n                        </p>\r\n                      </td>\r\n                    </tr>\r\n                    <tr style=\"padding: 10px 0\">\r\n                      <td\r\n                        align=\"left\"\r\n                        style=\"padding: 10px 0; font-weight: 700\"\r\n                      >\r\n                        <p\r\n                          style=\"\r\n                            color: #2579a9;\r\n                            font-weight: 800;\r\n                            font-weight: 14px;\r\n                          \"\r\n                        >\r\n                          Your account details\r\n                        </p>\r\n                        <p style=\"color: #555; font-size: 16px\">\r\n                          Use your email or username to login to your account\r\n                          and keep track of your of activities\r\n\r\n                          <br />\r\n                          Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.\r\n                        </p>\r\n                      </td>\r\n                    </tr>");

                /*if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    return RedirectToPage("RegisterConfirmation",
                        new { email = Input.Email, returnUrl = returnUrl });
                }
                else
                {*/
                //await _signInManager.SignInAsync(user, isPersistent: false);

                responseModel.Message = "User created a new account with password";
                responseModel.StatusCode = 200;
                return Ok(responseModel);
                //}
            }

            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
        }

        responseModel.Message = "Bad Request";
        responseModel.StatusCode = 400;
        var errors2 = ModelState.Values.SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage);
        return BadRequest(new { responseModel, Errors = errors2 });
    }

    [HttpGet]
    [Route(Constants.Get_User_By_Id)]
    //[Authorize(Roles = Constants.Role_Admin)]
    public async Task<IActionResult> GetUserById(string id)
    {
        var applicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == id);

        var formattedUser = new FormattedUserModel();
        formattedUser.id = applicationUser.Id;
        formattedUser.imageUrl = applicationUser.ImageUrl;
        formattedUser.email = applicationUser.Email;
        formattedUser.firstName = applicationUser.FirstName;
        formattedUser.lastName = applicationUser.LastName;
        formattedUser.phoneNumber = applicationUser.PhoneNumber;
        formattedUser.address = applicationUser.Address;
        formattedUser.position = applicationUser.Position;

        return Ok(formattedUser);
    }

    [HttpGet]
    [Route(Constants.Get_Logged_in_user_Endpoint)]
    //[Authorize(Roles = Constants.Role_Admin)]
    public async Task<IActionResult> GetLoggedInUser()
    {
        var responseModel = new ResponseModel();
        var userId = _userManager.GetUserId(User);
        if (User.Identity.IsAuthenticated)
        {
            var applicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == userId);

            var formattedUser = new FormattedUserModel();
            formattedUser.id = applicationUser.Id;
            formattedUser.imageUrl = applicationUser.ImageUrl;
            formattedUser.email = applicationUser.Email;
            formattedUser.firstName = applicationUser.FirstName;
            formattedUser.lastName = applicationUser.LastName;
            formattedUser.phoneNumber = applicationUser.PhoneNumber;
            formattedUser.address = applicationUser.Address;
            formattedUser.position = applicationUser.Position;


            return Ok(formattedUser);
        }

        responseModel.Message = "No user logged in";
        responseModel.StatusCode = 400;
        return BadRequest(responseModel);
    }

    [HttpGet]
    [Route(Constants.Get_All_User_Endpoint)]
    [Authorize(Roles = Constants.Role_Admin)]
    public async Task<IActionResult> GetAllUsers()
    {
        var formattedUserModelList = new List<FormattedUserModel>();
        var applicationUsers = _unitOfWork.ApplicationUser.GetAll();
        foreach (var applicationUser in applicationUsers)
        {
            var formattedUser = new FormattedUserModel();
            formattedUser.id = applicationUser.Id;
            formattedUser.imageUrl = applicationUser.ImageUrl;
            formattedUser.email = applicationUser.Email;
            formattedUser.firstName = applicationUser.FirstName;
            formattedUser.lastName = applicationUser.LastName;
            formattedUser.phoneNumber = applicationUser.PhoneNumber;
            formattedUser.address = applicationUser.Address;
            formattedUser.position = applicationUser.Position;

            formattedUserModelList.Add(formattedUser);
        }

        return Ok(formattedUserModelList);
    }

    [HttpDelete]
    [Route(Constants.Delete_A_User_Endpoint)]
    [Authorize(Roles = Constants.Role_Admin)]
    public async Task<IActionResult> DeleteAUser(string id)
    {
        var responseModel = new ResponseModel();
        if (id == null)
        {
            responseModel.Message = "Missing id field";
            responseModel.StatusCode = 400;

            ModelState.AddModelError(string.Empty, "The user id should be provided");
            var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);

            return BadRequest(new { responseModel, Errors = errors2 });
        }

        var applicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == id);
        if (applicationUser != null)
        {
            if (applicationUser.Id == _userManager.GetUserId(User))
            {
                responseModel.StatusCode = 400;
                responseModel.Message = "User cannot delete him/her self";
                return BadRequest(responseModel);
            }

            responseModel.StatusCode = 200;
            responseModel.Message = "User deleted successfully";
            _unitOfWork.ApplicationUser.Remove(applicationUser);
            _unitOfWork.Save();
            return Ok(responseModel);
        }

        responseModel.StatusCode = 400;
        responseModel.Message = "The user you are trying to delete does not exist";
        return BadRequest(responseModel);
    }


    [HttpGet]
    [Route(Constants.Confirm_Email_Endpoint)]
    public async Task<IActionResult> ConfirmEmail(string userId, string code)
    {
        var confirmEmail = new ConfirmEmail();
        var responseModel = new ResponseModel();
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
        confirmEmail.StatusMessage =
            result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
        if (result.Succeeded)
        {
            responseModel.StatusCode = 200;
            responseModel.Message = confirmEmail.StatusMessage;
            return Ok(responseModel);
        }

        responseModel.StatusCode = 400;
        responseModel.Message = confirmEmail.StatusMessage;
        foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
        var errors2 = ModelState.Values.SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage);
        return BadRequest(new { responseModel, Errors = errors2 });
    }

    // //[ValidateAntiForgeryToken]
    // [HttpPost]
    // [Route(Constants.Forgot_Password_Endpoint)]
    // public async Task<IActionResult> ForgotPassword([FromForm] ForgotPassword Input)
    // {
    //     ResponseModel responseModel = new ResponseModel();
    //     if (ModelState.IsValid)
    //     {
    //         var user = await _userManager.FindByEmailAsync(Input.Email);
    //         if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
    //         {
    //             responseModel.StatusCode = 200;
    //             responseModel.Message="If an account with the confirmed email address you provided exists in our system," +
    //                                   " we have sent you an email with instructions on how to reset your password." +
    //                                   " Please check your email and follow the instructions provided. " +
    //                                   "If you don't receive the email within a few minutes, " +
    //                                   "please check your spam folder or try again with a different email address.";
    //             return Ok(responseModel);
    //         }
    //         
    //         var code = await _userManager.GeneratePasswordResetTokenAsync(user);
    //         code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
    //        
    //         var callbackUrl =  $"{Request.Scheme}://{Request.Host}{Constants.Reset_Password_Endpoint}?code={code}";
    //
    //         await _emailSender.SendEmailAsync(
    //             Input.Email,
    //             "Reset Password",
    //             $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
    //
    //         responseModel.StatusCode = 200;
    //         responseModel.Message = "Please reset your password by clicking on the link sent to your email";
    //         return Ok(responseModel);
    //     }
    //     responseModel.Message = "Bad Request";
    //     responseModel.StatusCode = 400;
    //     var errors2 = ModelState.Values.SelectMany(v => v.Errors)
    //         .Select(e => e.ErrorMessage);
    //     return BadRequest(new {responseModel, Errors =errors2 });
    // }
    //
    // [HttpPost]
    // [Route(Constants.Reset_Password_Endpoint)]
    // public async Task<IActionResult> ResetPassword([FromForm] ResetPassword Input)
    // {
    //     ResponseModel responseModel = new ResponseModel();
    //     if (!ModelState.IsValid)
    //     {
    //         responseModel.Message = "Bad Request";
    //         responseModel.StatusCode = 400;
    //         var errors2 = ModelState.Values.SelectMany(v => v.Errors)
    //             .Select(e => e.ErrorMessage);
    //         return BadRequest(new {responseModel, Errors =errors2 });
    //     }
    //
    //     Input.Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Input.Code));
    //     var user = await _userManager.FindByEmailAsync(Input.Email);
    //     if (user == null)
    //     {
    //         // Don't reveal that the user does not exist
    //         responseModel.StatusCode = 404;
    //         responseModel.Message = "User does not exist.";
    //         return NotFound(responseModel);
    //     }
    //
    //     var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
    //     if (result.Succeeded)
    //     {
    //         responseModel.StatusCode = 200;
    //         responseModel.Message = "Your password has been reset.";
    //         return Ok(responseModel);
    //     }
    //     responseModel.Message = "Bad Request";
    //     responseModel.StatusCode = 400;
    //     
    //     foreach (var error in result.Errors)
    //     {
    //         ModelState.AddModelError(string.Empty, error.Description);
    //     }
    //     var errors = ModelState.Values.SelectMany(v => v.Errors)
    //         .Select(e => e.ErrorMessage);
    //     return BadRequest(new {responseModel, Errors =errors });
    // }
    //
    // [ValidateAntiForgeryToken]
    // [HttpPost]
    // [Route(Constants.Resend_Email_Verification_Endpoint)]
    // [Authorize(Roles = Constants.Role_Admin+","+Constants.Role_Staff)]
    // public async Task<IActionResult> ResendEmailVerification([FromForm] EmailVerification Input)
    // {
    //     ResponseModel responseModel = new ResponseModel();
    //     if (!ModelState.IsValid)
    //     {
    //         responseModel.StatusCode = 400;
    //         responseModel.Message = "Bad request";
    //         var errors = ModelState.Values.SelectMany(v => v.Errors)
    //             .Select(e => e.ErrorMessage);
    //         return BadRequest(new {responseModel, Errors =errors });
    //     }
    //
    //     var user = await _userManager.FindByEmailAsync(Input.Email);
    //     if (user == null)
    //     {
    //         ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
    //         responseModel.StatusCode = 200;
    //         responseModel.Message = "Verification email sent. Please check your email.";
    //         return Ok(responseModel);
    //     }
    //
    //     var userId = await _userManager.GetUserIdAsync(user);
    //     var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
    //     code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
    //     var callbackUrl =$"{Request.Scheme}://{Request.Host}{Constants.Confirm_Email_Endpoint}?userId={userId}&code={code}";
    //     
    //     await _emailSender.SendEmailAsync(
    //         Input.Email,
    //         "Confirm your email",
    //         $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
    //
    //     ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
    //     responseModel.StatusCode = 200;
    //     responseModel.Message = "Verification email sent. Please check your email.";
    //     return Ok(responseModel);
    // }
    //
    [HttpPost]
    [Route(Constants.Logout_Endpoint)]
    public async Task<IActionResult> Logout(string returnUrl = null)
    {
        await _signInManager.SignOutAsync();

        _logger.LogInformation("User logged out.");
        var responseModel = new ResponseModel();
        responseModel.StatusCode = 200;
        responseModel.Message = "User logged out";

        if (returnUrl != null)
            return Ok(new { responseModel }); //LocalRedirect(returnUrl);
        // This needs to be a redirect so that the browser performs a new
        // request and the identity for the user gets updated.
        return Ok(new { responseModel }); //return Redirect("");
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
            throw new NotSupportedException("The default UI requires a user store with email support.");

        return (IUserEmailStore<IdentityUser>)_userStore;
    }

    private string UploadImage(ImageFIleForm imageFile)
    {
        // string wwwRootPath = _hostEnvironment.WebRootPath;
        string ImageUrl;
        if (imageFile.Image != null)
        {
            // string fileName =imageFile.Image.FileName+ Guid.NewGuid().ToString();
            // var uploads = Path.Combine(wwwRootPath, @"profiles");
            // var extension = Path.GetExtension(imageFile.Image.FileName);
            //
            // using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
            // {
            //     imageFile.Image.CopyTo(fileStreams);
            // }

            var fileName = imageFile.Image.FileName + Guid.NewGuid();
            var fileUri = _blobUploader.UploadProfileImage(imageFile.Image, fileName);

            ImageUrl = fileUri;
            return ImageUrl;
        }

        return null;
    }

    public void SetCsrfToken(string role)
    {
        // var antiForgery = HttpContext.RequestServices.GetService<IAntiforgery>();
        // var tokens = antiForgery.GetAndStoreTokens(HttpContext);

        // Take request token (which is different from a cookie token)
        // var headerToken = tokens.RequestToken;
        // Set another cookie for a request token
        // Response.Cookies.Append("RequestVerificationToken", headerToken, new CookieOptions
        // {
        //     HttpOnly = false
        // });
        // HttpContext.Session.SetString("OauthTokenAccessToken", headerToken);
        Response.Cookies.Append("Role", role, new CookieOptions
        {
            HttpOnly = false
        });
        HttpContext.Session.SetString("Role", role);
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class InputModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}