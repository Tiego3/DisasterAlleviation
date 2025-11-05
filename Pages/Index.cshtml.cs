using DisasterAlleviation.Data;
using DisasterAlleviation.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisasterAlleviation.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // Dashboard summary data
        public decimal TotalMoneyDonations { get; set; }
        public int TotalGoodsReceived { get; set; }
        public int TotalDonors { get; set; }

        // Active disasters list
        public List<Disaster> ActiveDisasters { get; set; } = new();

        public async Task OnGetAsync()
        {
            // Donation summaries
            TotalMoneyDonations = await _context.MonetaryDonations
                .Select(d => (decimal?)d.Amount)
                .SumAsync() ?? 0m;

            TotalGoodsReceived = await _context.GoodsDonations
                .Select(d => (int?)d.ItemsCount)
                .SumAsync() ?? 0;

            TotalDonors = await _context.Donors.CountAsync();

            // Load active disasters
            ActiveDisasters = await _context.Disasters
                .Where(d => d.Status == "Active")
                .OrderByDescending(d => d.StartDate)
                .ToListAsync();
        }
    }
}
