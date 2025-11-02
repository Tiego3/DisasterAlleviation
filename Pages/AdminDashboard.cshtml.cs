using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using DisasterAlleviation.Data;

namespace DisasterAlleviation.Pages
{
    [Authorize(Roles = "Admin")]
    public class AdminDashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public int TotalMonetary { get; set; }
        public int TotalGoods { get; set; }
        public int TotalCategories { get; set; }
        public int TotalDonors { get; set; }

        public AdminDashboardModel(ApplicationDbContext context)
    {
            _context = context;
        }

        public void OnGet()
        {
            TotalMonetary = _context.MonetaryDonations.Count();
            TotalGoods = _context.GoodsDonations.Count();
            TotalCategories = _context.Categories.Count();
            TotalDonors = _context.Donors.Count();
        }
    }
}
