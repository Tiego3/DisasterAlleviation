using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DisasterAlleviation.Data;
using DisasterAlleviation.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DisasterAlleviation.Pages
{
    public class DonateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public MonetaryDonationForm MonetaryForm { get; set; } = new();

        [BindProperty]
        public GoodsDonationForm GoodsForm { get; set; } = new();

        public List<Category> Categories { get; set; } = new();

        public DonateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        private void PopulateCategories()
        {
            // Fetch from DB if available, else seed fallback
            Categories = _context.Categories.Any()
                ? _context.Categories.ToList()
                : new List<Category>
                {
                    new Category { Id = 1, Name = "Clothing" },
                    new Category { Id = 2, Name = "Food" },
                    new Category { Id = 3, Name = "Medical Supplies" }
                };
        }

        public void OnGet()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.AddRange(new List<Category>
        {
            new Category { Name = "Clothing" },
            new Category { Name = "Food" },
            new Category { Name = "Medical Supplies" }
        });
                _context.SaveChanges();
            }

            Categories = _context.Categories.ToList();
        }

        public async Task<IActionResult> OnPostMonetaryDonationAsync()
        {
            if (!ModelState.IsValid)
            {
                PopulateCategories();
                return Page();
            }

            // Save to database
            var donation = new MonetaryDonation
            {
                DonorName = MonetaryForm.IsAnonymous ? "Anonymous" : MonetaryForm.DonorName,
                Amount = MonetaryForm.Amount,
            };

            _context.MonetaryDonations.Add(donation);
            await _context.SaveChangesAsync();

            // Redirect to home page after success
            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPostGoodsDonationAsync()
        {
            if (!ModelState.IsValid)
            {
                PopulateCategories();
                return Page();
            }

            var goods = new GoodsDonation
            {
                DonorName = GoodsForm.IsAnonymous ? "Anonymous" : GoodsForm.DonorName,
                CategoryId = GoodsForm.CategoryId,
                ItemsCount = GoodsForm.ItemsCount,
                Description = GoodsForm.Description
            };

            _context.GoodsDonations.Add(goods);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }

    public class MonetaryDonationForm
    {
        public bool IsAnonymous { get; set; }
        public string DonorName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }

    public class GoodsDonationForm
    {
        public bool IsAnonymous { get; set; }
        public string DonorName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public int ItemsCount { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
