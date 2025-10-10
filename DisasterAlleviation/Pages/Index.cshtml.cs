using DisasterAlleviation.Data;
using Microsoft.AspNetCore.Identity; // This 'using' isn't needed for the code provided, but kept it as it was in the original
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System; // This 'using' isn't needed for the code provided, but kept it as it was in the original
using System.ComponentModel.DataAnnotations; // This 'using' isn't needed for the code provided, but kept it as it was in the original
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

        // Properties to hold the aggregated data for the dashboard
        public decimal TotalMoneyDonations { get; set; }
        public int TotalGoodsReceived { get; set; }
        public int TotalDonors { get; set; }

        public async Task OnGetAsync()
        {
            TotalMoneyDonations = await _context.MonetaryDonations
                .Select(d => (decimal?)d.Amount)
                .SumAsync() ?? 0m;

            TotalGoodsReceived = await _context.GoodsDonations
                .Select(d => (int?)d.ItemsCount)
                .SumAsync() ?? 0;

            TotalDonors = await _context.MonetaryDonations
                .Where(d => !string.IsNullOrEmpty(d.DonorName))
                .Select(d => d.DonorName!)
                .Distinct()
                .CountAsync();
        }
    }
}