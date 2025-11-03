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
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public VolunteerModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required, Display(Name = "Full Name")]
            public string FullName { get; set; }

            [Required, EmailAddress, Display(Name = "Email Address")]
            public string Email { get; set; }

            [Required, Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }

            [Required, Display(Name = "Location")]
            public string Location { get; set; }

            [Required, Display(Name = "Availability")]
            public string Availability { get; set; }

            [Required, Display(Name = "Skills & Expertise")]
            public string Skills { get; set; }

            [Display(Name = "Previous Volunteer Experience")]
            public string? PreviousExperience { get; set; }

            [Display(Name = "Available Dates")]
            [DataType(DataType.Date)]
            public DateTime? AvailableDates { get; set; }

            [Required, Display(Name = "Emergency Contact")]
            public string EmergencyContact { get; set; }

            [Display(Name = "I have reliable transportation")]
            public bool HasTransportation { get; set; }

            [Display(Name = "I am willing to travel for disaster response")]
            public bool WillingToTravel { get; set; }

            [Display(Name = "Additional Information")]
            public string? AdditionalInfo { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var user = await _userManager.GetUserAsync(User);

            var volunteer = new Volunteer
            {
                UserId = user?.Id,
                FullName = Input.FullName,
                Email = Input.Email,
                PhoneNumber = Input.PhoneNumber,
                Location = Input.Location,
                Availability = Input.Availability,
                Skills = Input.Skills,
                PreviousExperience = Input.PreviousExperience,
                AvailableDates = Input.AvailableDates,
                EmergencyContact = Input.EmergencyContact,
                HasTransportation = Input.HasTransportation,
                WillingToTravel = Input.WillingToTravel,
                AdditionalInfo = Input.AdditionalInfo,

                // ✅ Set backend-only fields safely
                Status = "Pending", // default when submitted
                CanTravel = Input.WillingToTravel, // optional: mirror their willingness
                DateApplied = DateTime.UtcNow
            };

            _context.Volunteers.Add(volunteer);
            await _context.SaveChangesAsync();

            TempData["VolunteerSuccess"] = "Thank you! Your volunteer application has been successfully submitted.";

            return RedirectToPage("/Index");
        }
    }
}
