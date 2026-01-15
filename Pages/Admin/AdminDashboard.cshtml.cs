using DisasterAlleviation.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DisasterAlleviation.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminDashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AdminDashboardModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // Summary card values
        public decimal CurrentBalance { get; set; }
        public int GoodsAvailable { get; set; }
        public int ActiveDisasters { get; set; }
        public int TotalDonors { get; set; }

        public async Task OnGetAsync()
        {
            // 1. Total Donations (all money currently in system)
            CurrentBalance = await _context.MonetaryDonations
                .Select(d => (decimal?)d.Amount)
                .SumAsync() ?? 0m;

            // 2. Goods Available (all goods currently recorded)
            GoodsAvailable = await _context.GoodsDonations
                .Select(g => (int?)g.ItemsCount)
                .SumAsync() ?? 0;

            // 3. Active Disasters
            ActiveDisasters = await _context.Disasters
                .CountAsync(d => d.Status == "Active");

            // 4. Total Donors
            TotalDonors = await _context.Donors.CountAsync();
        }
    }
}
