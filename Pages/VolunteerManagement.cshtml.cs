using DisasterAlleviation.Data;
using DisasterAlleviation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DisasterAlleviation.Pages
{
    public class VolunteerManagementModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public VolunteerManagementModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Volunteer> Volunteers { get; set; }
        public IList<TaskModel> Tasks { get; set; }
        public IList<IncidentReport> Incidents { get; set; }

        public async Task OnGetAsync()
        {
            Volunteers = await _context.Volunteers.ToListAsync();
            Tasks = await _context.Tasks.ToListAsync();
            Incidents = await _context.IncidentReports.ToListAsync();
        }

        public async Task<IActionResult> OnPostAssignTaskAsync(int VolunteerId, int TaskId)
        {
            var volunteer = await _context.Volunteers.Include(v => v.Task).FirstOrDefaultAsync(v => v.Id == VolunteerId);
            var task = await _context.Tasks.FindAsync(TaskId);

            if (volunteer == null || task == null)
            {
                return NotFound();
            }

            if (volunteer.IsTaskAssigned)
            {
                ModelState.AddModelError(string.Empty, "Volunteer already has a task assigned and cannot be assigned another one.");
                return Page();
            }

            volunteer.TaskId = TaskId;
            _context.Volunteers.Update(volunteer);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostMatchIncidentAsync(int VolunteerId, int IncidentId)
        {
            var volunteer = await _context.Volunteers.FindAsync(VolunteerId);
            var incident = await _context.IncidentReports.FindAsync(IncidentId);

            if (volunteer != null && incident != null)
            {
                volunteer.IncidentId = IncidentId;
                _context.Volunteers.Update(volunteer);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
