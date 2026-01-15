using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using DisasterAlleviation.Data;
using DisasterAlleviation.Models;
using Microsoft.EntityFrameworkCore;

namespace DisasterAlleviation.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminGoodsDropoffsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AdminGoodsDropoffsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<GoodsDropoffViewModel> Dropoffs { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
        public int TotalDropoffs { get; set; }
        public int ScheduledCount { get; set; }
        public int PendingCount { get; set; }
        public int CompletedCount { get; set; }

        public async Task OnGetAsync()
        {
            Categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();

            Dropoffs = await _context.GoodsDonations
                .Include(d => d.Category)
                .Include(d => d.Donor)
                .OrderByDescending(d => d.DropoffDateTime ?? d.DateDonated)
                .Select(d => new GoodsDropoffViewModel
                {
                    Id = d.Id,
                    ReferenceNumber = d.ReferenceNumber ?? "N/A",
                    DonorName = d.Donor.IsAnonymous ? "Anonymous" : d.DonorName,
                    ContactInfo = d.Donor.Email ?? "N/A",
                    CategoryName = d.Category.Name,
                    ItemsCount = d.ItemsCount,
                    Description = d.Description,
                    DropoffDateTime = d.DropoffDateTime,
                    DropoffStatus = d.DropoffStatus,
                    CompletedAt = d.CompletedAt,
                    AdminNotes = d.AdminNotes
                })
                .ToListAsync();

            TotalDropoffs = Dropoffs.Count;
            ScheduledCount = Dropoffs.Count(d => d.DropoffStatus == "Scheduled");
            PendingCount = Dropoffs.Count(d => d.DropoffStatus == "Pending");
            CompletedCount = Dropoffs.Count(d => d.DropoffStatus == "Completed");
        }

        public async Task<IActionResult> OnGetDetailsAsync(int id)
        {
            var donation = await _context.GoodsDonations
                .Include(d => d.Category)
                .Include(d => d.Donor)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (donation == null) return NotFound();

            var dto = new
            {
                id = donation.Id,
                referenceNumber = donation.ReferenceNumber ?? "N/A",
                donorName = donation.Donor?.IsAnonymous == true ? "Anonymous" : donation.DonorName,
                contactInfo = donation.Donor?.Email ?? "N/A",
                categoryName = donation.Category?.Name ?? "Unknown",
                itemsCount = donation.ItemsCount,
                description = donation.Description,
                dropoffDateTime = donation.DropoffDateTime?.ToString("o"),
                dropoffStatus = donation.DropoffStatus,
                completedAt = donation.CompletedAt?.ToString("o"),
                adminNotes = donation.AdminNotes
            };

            return new JsonResult(dto);
        }

        public async Task<IActionResult> OnPostCompleteAsync(int id, string? notes)
        {
            var donation = await _context.GoodsDonations.FindAsync(id);
            if (donation == null)
            {
                return NotFound(new { success = false, error = "Donation not found" });
            }

            donation.DropoffStatus = "Completed";
            donation.CompletedAt = DateTime.UtcNow;
            donation.AdminNotes = notes;

            await _context.SaveChangesAsync();

            return new JsonResult(new { success = true });
        }

        public async Task<IActionResult> OnPostRecordOnSiteAsync(string refNumber, string? notes)
        {
            var donation = await _context.GoodsDonations
                .FirstOrDefaultAsync(d => d.ReferenceNumber == refNumber);

            if (donation == null)
            {
                return NotFound(new { success = false, error = "Reference number not found. Please verify the number." });
            }

            if (donation.DropoffStatus == "Completed")
            {
                return BadRequest(new { success = false, error = "This drop-off has already been completed." });
            }

            donation.DropoffStatus = "Completed";
            donation.CompletedAt = DateTime.UtcNow;
            donation.AdminNotes = notes;

            await _context.SaveChangesAsync();

            return new JsonResult(new { success = true });
        }

 
    }
}
