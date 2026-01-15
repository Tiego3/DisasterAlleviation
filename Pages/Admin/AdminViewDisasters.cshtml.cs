using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DisasterAlleviation.Data;
using DisasterAlleviation.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisasterAlleviation.Pages.Admin
{
    public class AdminViewDisastersModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public AdminViewDisastersModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Disaster> AllDisasters { get; set; } = new();

        public async Task OnGetAsync()
        {
            AllDisasters = await _context.Disasters
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostCloseAsync(int id)
        {
            var d = await _context.Disasters.FindAsync(id);
            if (d == null) return NotFound();

            d.Status = "Closed";
            await _context.SaveChangesAsync();
            return new JsonResult(new { success = true });
        }

        public async Task<IActionResult> OnGetDetailsAsync(int id)
        {
            var d = await _context.Disasters
                .Include(x => x.Volunteers)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (d == null) return NotFound();

            var dto = new
            {
                id = d.Id,
                typeOfDisaster = d.TypeOfDisaster,
                location = d.Location,
                description = d.Description,
                startDate = d.StartDate.ToString("o"),
                endDate = d.EndDate?.ToString("o"),
                status = d.Status,
                createdAt = d.CreatedAt.ToString("o"),
                requiredAidTypes = d.RequiredAidTypes,
                volunteers = d.Volunteers?.Select(v => new {
                    id = v.Id,
                    fullName = v.FullName,
                    email = v.Email,
                    phoneNumber = v.PhoneNumber,
                    location = v.Location,
                    status = v.Status
                }) // ?? new object[] { },
                //resources = d.Resources?.Select(r => new {
                //    id = r.Id,
                //    resourceName = r.ResourceName,
                //    quantity = r.Quantity,
                //    unit = r.Unit,
                //    resourceType = r.ResourceType
                //}) ?? new object[] { }
            };

            return new JsonResult(dto);
        }
    }
}
