using DisasterAlleviation.Data;
using DisasterAlleviation.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DisasterAlleviation.Pages
{
    public class DonationManagentModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DonationManagentModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Donation> Donations { get; set; }

        public async Task OnGetAsync()
        {
            Donations = await _context.Donations.ToListAsync();
        }
    }
}
