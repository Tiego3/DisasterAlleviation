using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using DisasterAlleviation.Data;
using DisasterAlleviation.Models;
using Microsoft.EntityFrameworkCore;

namespace DisasterAlleviation.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminCategoriesModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AdminCategoriesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Category> Categories { get; set; } = new();
        public Dictionary<int, int> CategoryDonationCounts { get; set; } = new();
        public int TotalDonations { get; set; }
        public string? MostUsedCategory { get; set; }

        public async Task OnGetAsync()
        {
            Categories = await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();

            // Calculate donation counts per category
            foreach (var cat in Categories)
            {
                CategoryDonationCounts[cat.Id] = await _context.GoodsDonations
                    .CountAsync(d => d.CategoryId == cat.Id);
            }

            // Calculate stats
            TotalDonations = CategoryDonationCounts.Values.Sum();

            if (CategoryDonationCounts.Any())
            {
                var mostUsedId = CategoryDonationCounts
                    .OrderByDescending(kvp => kvp.Value)
                    .First()
                    .Key;
                MostUsedCategory = Categories.FirstOrDefault(c => c.Id == mostUsedId)?.Name;
            }
        }

        public async Task<IActionResult> OnPostCreateAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest(new { success = false, error = "Category name is required" });
            }

            var category = new Category
            {
                Name = name.Trim()
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return new JsonResult(new { success = true, id = category.Id });
        }

        public async Task<IActionResult> OnPostUpdateAsync(int id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest(new { success = false, error = "Category name is required" });
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound(new { success = false, error = "Category not found" });
            }

            category.Name = name.Trim();
            await _context.SaveChangesAsync();

            return new JsonResult(new { success = true });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound(new { success = false, error = "Category not found" });
            }

            // Check if category has associated donations
            var hasRelatedDonations = await _context.GoodsDonations
                .AnyAsync(d => d.CategoryId == id);

            if (hasRelatedDonations)
            {
                return BadRequest(new { success = false, error = "Cannot delete category with existing donations" });
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return new JsonResult(new { success = true });
        }
    }
}