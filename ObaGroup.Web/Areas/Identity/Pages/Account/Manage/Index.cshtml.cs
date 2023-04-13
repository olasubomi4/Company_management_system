// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ObaGoupDataAccess.Repository;
using ObaGoupDataAccess.Repository.IRepository;
using ObaGroupModel;

namespace Oba_group2.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

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
            [Phone]
            [MaxLength (11,ErrorMessage = "Phone number lenght cannot be more than 11")]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            
            [Display(Name = "First name")]
            public string FirstName { get; set; }
            
            [Display(Name = "Last name")]
            public string LastName { get; set; }
            
            [Display(Name = "Address")]
            public string Address { get; set; }
                 
            [Display(Name = "Position")]
            public string Position { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = _unitOfWork.ApplicationUser.GetFirstOrDefault(u=>u.Id==user.Id).PhoneNumber;
            var firstName = _unitOfWork.ApplicationUser.GetFirstOrDefault(u=>u.Id==user.Id).FirstName;
            var lastName = _unitOfWork.ApplicationUser.GetFirstOrDefault(u=>u.Id==user.Id).LastName;
            var address = _unitOfWork.ApplicationUser.GetFirstOrDefault(u=>u.Id==user.Id).Address;
            var position = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == user.Id).Position;
            
            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FirstName = firstName,
                LastName = lastName,
                Address = address,
                Position=position
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            ApplicationUser applicationUser = new ApplicationUser();
            var phoneNumber = _unitOfWork.ApplicationUser.GetFirstOrDefault(u=>u.Id==user.Id).PhoneNumber;
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }

        
            }

            await _signInManager.RefreshSignInAsync(user);
            applicationUser.Id = user.Id;
            applicationUser.FirstName = Input.FirstName;
            applicationUser.LastName = Input.LastName;
            applicationUser.Address = Input.Address;
            applicationUser.Position = Input.Position;
            _unitOfWork.ApplicationUser.Update(applicationUser);
            _unitOfWork.Save();
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
