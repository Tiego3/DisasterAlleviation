using DisasterAlleviation.Data;
using DisasterAlleviation.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviation.Pages
{
    public class VolunteerModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context; // Inject DbContext

        public VolunteerModel(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context; // Initialize DbContext
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Name is required.")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Contact information is required.")]
            public string ContactInfo { get; set; }

            [Required(ErrorMessage = "Skills are required.")]
            public string Skills { get; set; }

            [Required(ErrorMessage = "Availability is required.")]
            public DateTime Availability { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var user = await _userManager.GetUserAsync(User); // Get the currently logged-in user

            // Create a new Volunteer record
            var volunteer = new Volunteer
            {
                UserId = user.Id, // Associate with the IdentityUser's ID
                Name = Input.Name,
                ContactInfo = Input.ContactInfo,
                Skills = Input.Skills,
                Availability = Input.Availability,
               
            };

            // Save volunteer details to the database
            _context.Volunteers.Add(volunteer);
            await _context.SaveChangesAsync(); // Save changes asynchronously

            return RedirectToPage("VolunteerDashboard");
        }
    }


}
