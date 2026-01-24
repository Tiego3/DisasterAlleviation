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

            var totalMonetary = await _context.MonetaryDonations.SumAsync(d => (decimal?)d.Amount) ?? 0m;
            var allocatedMonetary = await _context.ResourceAllocations
                .Where(r => r.ResourceType == "Monetary")
                .SumAsync(r => (decimal?)r.Quantity) ?? 0m;
            CurrentBalance = totalMonetary - allocatedMonetary;

            // Calculate available (unallocated) goods - only count completed drop-offs
            var totalGoods = await _context.GoodsDonations
                .Where(d => d.DropoffStatus == "Completed")
                .SumAsync(d => (int?)d.ItemsCount) ?? 0;
            var allocatedGoods = await _context.ResourceAllocations
                .Where(r => r.ResourceType == "Goods")
                .SumAsync(r => (int?)r.Quantity) ?? 0;
            GoodsAvailable = totalGoods - allocatedGoods;

            // 3. Active Disasters
            ActiveDisasters = await _context.Disasters
                .CountAsync(d => d.Status == "Active");

            // 4. Total Donors
            TotalDonors = await _context.Donors.CountAsync();
        }
    }
}
