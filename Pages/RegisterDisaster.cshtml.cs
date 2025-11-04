using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DisasterAlleviation.Data;
using DisasterAlleviation.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace DisasterAlleviation.Pages
{
    public class RegisterDisasterModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public RegisterDisasterModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public List<string> AidTypes { get; set; } = new()
        {
            "Water provision",
            "Food",
            "Clothing",
            "Medical supplies",
            "Shelter materials",
            "Emergency supplies",
            "Transportation",
            "Communication equipment"
        };

        public class InputModel
        {
            public string TypeOfDisaster { get; set; }
            public string Location { get; set; }
            public string Description { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public List<string> RequiredAidTypes { get; set; } = new();
            public string? OtherAid { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Return bad request if invalid for AJAX flow
                return BadRequest(ModelState);
            }

            var aidList = Input.RequiredAidTypes ?? new List<string>();
            if (!string.IsNullOrWhiteSpace(Input.OtherAid))
                aidList.Add(Input.OtherAid);

            var disaster = new Disaster
            {
                TypeOfDisaster = Input.TypeOfDisaster,
                Location = Input.Location,
                Description = Input.Description,
                StartDate = Input.StartDate,
                EndDate = Input.EndDate,
                RequiredAidTypes = string.Join(", ", aidList),
                CreatedAt = DateTime.UtcNow,
                Status = "Active",
                Volunteers = new List<Volunteer>()
              //  Resources = new List<Resource>()
            };

            _context.Disasters.Add(disaster);
            await _context.SaveChangesAsync();

            // Return JSON for AJAX client
            return new JsonResult(new { success = true, redirectUrl = Url.Page("/AdminDashboard") });
        }
    }
}
