using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DisasterAlleviation.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DisasterAlleviation.Pages
{
    public class IncidentManagementModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IncidentManagementModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<IncidentReport> IncidentReports { get; set; }

        public async Task OnGetAsync()
        {
            IncidentReports = await _context.IncidentReports
            .OrderByDescending(report => report.ReportDate)
            .ToListAsync();
        }
    }
}
