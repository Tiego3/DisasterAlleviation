using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace DisasterAlleviation.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ProfileModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Email is required.")]
            [EmailAddress(ErrorMessage = "Invalid email format.")]
            public string Email { get; set; }

            [DataType(DataType.Password)]
            [StringLength(100, ErrorMessage = "The password must be at least {2} characters long.", MinimumLength = 6)]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                Input = new InputModel
                {
                    Email = user.Email
                };
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Update email
            if (user.Email != Input.Email)
            {
                user.Email = Input.Email;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    ErrorMessage = "Failed to update email.";
                    return Page();
                }
            }

            // Update password if provided
            if (!string.IsNullOrEmpty(Input.NewPassword))
            {
                var passwordChangeResult = await _userManager.ChangePasswordAsync(user, Input.NewPassword, Input.NewPassword);
                if (!passwordChangeResult.Succeeded)
                {
                    ErrorMessage = "Failed to change password.";
                    return Page();
                }
            }

            SuccessMessage = "Profile updated successfully.";
            return RedirectToPage("Dashboard");
        }
    }
}
