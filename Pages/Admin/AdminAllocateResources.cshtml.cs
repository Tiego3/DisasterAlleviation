using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using DisasterAlleviation.Data;
using DisasterAlleviation.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviation.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminAllocateResourcesModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AdminAllocateResourcesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public List<Disaster> ActiveDisasters { get; set; } = new();
        public decimal AvailableMonetary { get; set; }
        public int AvailableGoods { get; set; }

        public class InputModel
        {
            [Required]
            public int DisasterId { get; set; }

            [Required]
            public string ResourceType { get; set; }

            public decimal? MonetaryAmount { get; set; }

            public string? GoodsName { get; set; }
            public int? GoodsQuantity { get; set; }
            public string? GoodsUnit { get; set; }
            public string? Notes { get; set; }
        }

        public async Task OnGetAsync()
        {
            ActiveDisasters = await _context.Disasters
                .Where(d => d.Status == "Active")
                .OrderByDescending(d => d.StartDate)
                .ToListAsync();

            AvailableMonetary = await _context.MonetaryDonations
                .Where(d => !_context.ResourceAllocations.Any(r => r.SourceType == "Monetary" && r.SourceId == d.Id))
                .SumAsync(d => (decimal?)d.Amount) ?? 0m;

            AvailableGoods = await _context.GoodsDonations
                .Where(d => !_context.ResourceAllocations.Any(r => r.SourceType == "Goods" && r.SourceId == d.Id))
                .SumAsync(d => (int?)d.ItemsCount) ?? 0;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Input.ResourceType == "Monetary")
            {
                ModelState.Remove("Input.GoodsName");
                ModelState.Remove("Input.GoodsQuantity");
                if (!Input.MonetaryAmount.HasValue || Input.MonetaryAmount <= 0)
                {
                    ModelState.AddModelError("Input.MonetaryAmount", "Please enter a valid amount.");
                }
            }
            else
            {
                ModelState.Remove("Input.MonetaryAmount");
                if (string.IsNullOrWhiteSpace(Input.GoodsName))
                {
                    ModelState.AddModelError("Input.GoodsName", "Item name is required.");
                }
                if (!Input.GoodsQuantity.HasValue || Input.GoodsQuantity <= 0)
                {
                    ModelState.AddModelError("Input.GoodsQuantity", "Please enter a valid quantity.");
                }
            }

            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            var allocation = new ResourceAllocation
            {
                DisasterId = Input.DisasterId,
                ResourceType = Input.ResourceType,
                AllocatedAt = DateTime.UtcNow,
                Notes = Input.Notes
            };

            if (Input.ResourceType == "Monetary")
            {
                allocation.ResourceName = "Monetary Donation";
                allocation.Quantity = Input.MonetaryAmount.Value;
                allocation.Unit = "ZAR";
            }
            else
            {
                allocation.ResourceName = Input.GoodsName;
                allocation.Quantity = Input.GoodsQuantity.Value;
                allocation.Unit = Input.GoodsUnit ?? "items";
            }

            _context.ResourceAllocations.Add(allocation);
            await _context.SaveChangesAsync();

            return new JsonResult(new { success = true, redirectUrl = "/Admin/AdminDashboard" });
        }
    }
}