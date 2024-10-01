using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DisasterAlleviation.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviation.Pages
{
    public class ReportIncidentModel : PageModel
    {
        private readonly ApplicationDbContext _context;
       

        public ReportIncidentModel(ApplicationDbContext context)
        {
            _context = context;
           
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Incident Type is required")]
            public string IncidentType { get; set; }

            [Required(ErrorMessage = "Location is required")]
            public string Location { get; set; }

            [Required(ErrorMessage = "Severity is required")]
            public string Severity { get; set; }

            [Required(ErrorMessage = "Description is required")]
            [StringLength(500, ErrorMessage = "Description can't be longer than 500 characters")]
            public string Description { get; set; }

            public DateTime ReportDate { get; set; }
        }



        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var report = new IncidentReport
            {
                IncidentType = Input.IncidentType,
                Location = Input.Location,
                Severity = Input.Severity,
                Description = Input.Description,
                ReportDate = Input.ReportDate
            };

            _context.IncidentReports.Add(report);
            await _context.SaveChangesAsync();

            return RedirectToPage("Dashboard"); 
        }

    }
}
