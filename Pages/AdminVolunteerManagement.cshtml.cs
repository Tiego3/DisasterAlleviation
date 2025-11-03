using DisasterAlleviation.Data;
using DisasterAlleviation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisasterAlleviation.Pages
{
    public class AdminVolunteerManagementModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public AdminVolunteerManagementModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Volunteer> Volunteers { get; set; } = new();
        public int TotalApplications { get; set; }
        public int ApprovedCount { get; set; }
        public int PendingCount { get; set; }
        public int CanTravelCount { get; set; }

        public async Task OnGetAsync()
        {
            Volunteers = await _context.Volunteers.OrderByDescending(v => v.Id).ToListAsync();
            TotalApplications = Volunteers.Count;
            ApprovedCount = Volunteers.Count(v => v.Status == "Approved");
            PendingCount = Volunteers.Count(v => v.Status == "Pending");
            CanTravelCount = Volunteers.Count(v => v.WillingToTravel);
        }

        public async Task<JsonResult> OnGetVolunteerDetailsAsync(int id)
        {
            var volunteer = await _context.Volunteers.FindAsync(id);
            return new JsonResult(volunteer);
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync(int id, string status)
        {
            var volunteer = await _context.Volunteers.FindAsync(id);
            if (volunteer == null)
                return NotFound();

            volunteer.Status = status;
            await _context.SaveChangesAsync();

            return new JsonResult(new { success = true });
        }
    }
}
