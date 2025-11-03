using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DisasterAlleviation.Data;
using DisasterAlleviation.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DisasterAlleviation.Pages.Admin
{
    public class VolunteerDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public VolunteerDetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Volunteer Volunteer { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Volunteer = await _context.Volunteers.FirstOrDefaultAsync(v => v.Id == id);

            if (Volunteer == null)
                return NotFound();

            return Page();
        }

        // ✅ Approve volunteer
        public async Task<IActionResult> OnPostApproveAsync(int id)
        {
            var volunteer = await _context.Volunteers.FindAsync(id);
            if (volunteer == null) return NotFound();

            volunteer.Status = "Approved";
            await _context.SaveChangesAsync();

            TempData["Message"] = $"{volunteer.FullName} has been approved.";
            return RedirectToPage("/Admin/AdminVolunteerManagement");
        }

        // ❌ Reject volunteer
        public async Task<IActionResult> OnPostRejectAsync(int id)
        {
            var volunteer = await _context.Volunteers.FindAsync(id);
            if (volunteer == null) return NotFound();

            volunteer.Status = "Rejected";
            await _context.SaveChangesAsync();

            TempData["Message"] = $"{volunteer.FullName} has been rejected.";
            return RedirectToPage("/Admin/AdminVolunteerManagement");
        }
    }
}
