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

    
            var totalMonetary = await _context.MonetaryDonations.SumAsync(d => (decimal?)d.Amount) ?? 0m;

            // Calculate allocated monetary funds
            var allocatedMonetary = await _context.ResourceAllocations
                .Where(r => r.ResourceType == "Monetary")
                .SumAsync(r => (decimal?)r.Quantity) ?? 0m;

            AvailableMonetary = totalMonetary - allocatedMonetary;

            // Calculate total completed goods donations
            var totalGoods = await _context.GoodsDonations
                .Where(d => d.DropoffStatus == "Completed")
                .SumAsync(d => (int?)d.ItemsCount) ?? 0;

            // Calculate allocated goods
            var allocatedGoods = await _context.ResourceAllocations
                .Where(r => r.ResourceType == "Goods")
                .SumAsync(r => (int?)r.Quantity) ?? 0;

            AvailableGoods = totalGoods - allocatedGoods;
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

            // VALIDATE SUFFICIENT RESOURCES BEFORE ALLOCATING
            if (Input.ResourceType == "Monetary")
            {
                // Check available monetary funds
                var totalMonetary = await _context.MonetaryDonations.SumAsync(d => (decimal?)d.Amount) ?? 0m;
                var allocatedMonetary = await _context.ResourceAllocations
                    .Where(r => r.ResourceType == "Monetary")
                    .SumAsync(r => (decimal?)r.Quantity) ?? 0m;
                var availableMonetary = totalMonetary - allocatedMonetary;

                if (Input.MonetaryAmount.Value > availableMonetary)
                {
                    ModelState.AddModelError("Input.MonetaryAmount",
                        $"Insufficient funds. Only R{availableMonetary:N2} available.");
                    await OnGetAsync();
                    return Page();
                }
            }
            else // Goods
            {
                // Check available goods
                var totalGoods = await _context.GoodsDonations
                    .Where(d => d.DropoffStatus == "Completed")
                    .SumAsync(d => (int?)d.ItemsCount) ?? 0;
                var allocatedGoods = await _context.ResourceAllocations
                    .Where(r => r.ResourceType == "Goods")
                    .SumAsync(r => (int?)r.Quantity) ?? 0;
                var availableGoods = totalGoods - allocatedGoods;

                if (Input.GoodsQuantity.Value > availableGoods)
                {
                    ModelState.AddModelError("Input.GoodsQuantity",
                        $"Insufficient goods. Only {availableGoods} items available.");
                    await OnGetAsync();
                    return Page();
                }
            }

            // CREATE SINGLE ALLOCATION RECORD (no duplicates)
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