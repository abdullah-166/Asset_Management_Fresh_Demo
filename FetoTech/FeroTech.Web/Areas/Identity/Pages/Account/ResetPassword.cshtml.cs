<<<<<<< Updated upstream
﻿using System.ComponentModel.DataAnnotations;
using System.Text;
=======
using System.ComponentModel.DataAnnotations;
>>>>>>> Stashed changes
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
<<<<<<< Updated upstream
using Microsoft.AspNetCore.WebUtilities;
=======
>>>>>>> Stashed changes

namespace FeroTech.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
<<<<<<< Updated upstream
        private readonly UserManager<IdentityRole> _userManager;

        public ResetPasswordModel(UserManager<IdentityRole> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }
=======
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<ResetPasswordModel> _logger;

        public ResetPasswordModel(UserManager<IdentityUser> userManager, ILogger<ResetPasswordModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();
>>>>>>> Stashed changes

        public class InputModel
        {
            [Required]
            [EmailAddress]
<<<<<<< Updated upstream
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            public string Code { get; set; }
=======
            [Display(Name = "Email")]
            public string Email { get; set; } = string.Empty;

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string Password { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; } = string.Empty;

            [Required]
            public string Code { get; set; } = string.Empty;
>>>>>>> Stashed changes
        }

        public IActionResult OnGet(string code = null)
        {
            if (code == null)
            {
<<<<<<< Updated upstream
                return BadRequest("A code must be supplied for password reset.");
            }
            else
            {
                Input = new InputModel
                {
                    Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
                };
                return Page();
            }
=======
                _logger.LogWarning("Password reset attempted without a code");
                return BadRequest("A code must be supplied for password reset.");
            }

            Input = new InputModel
            {
                Code = code
            };
            return Page();
>>>>>>> Stashed changes
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
<<<<<<< Updated upstream
=======
                _logger.LogWarning("Password reset attempted for non-existent user: {Email}", Input.Email);
>>>>>>> Stashed changes
                // Don't reveal that the user does not exist
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
<<<<<<< Updated upstream
=======
                _logger.LogInformation("Password reset successful for user: {Email}", Input.Email);
>>>>>>> Stashed changes
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
<<<<<<< Updated upstream
=======
                _logger.LogWarning("Password reset failed for user {Email}: {Error}", Input.Email, error.Description);
>>>>>>> Stashed changes
            }
            return Page();
        }
    }
}