using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DisasterAlleviation.Data;
using DisasterAlleviation.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using DisasterAlleviation.Models.Forms;

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

        public string ActiveTab { get; set; } = "monetary"; // Default tab

        public DonateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        private void PopulateCategories()
        {
            Categories = _context.Categories.Any()
                ? _context.Categories.ToList()
                : new List<Category>
                {
                    new Category { Id = 1, Name = "Clothing" },
                    new Category { Id = 2, Name = "Food" },
                    new Category { Id = 3, Name = "Medical Supplies" }
                };
        }

        public void OnGet(string? tab)
        {
            // Ensure categories exist
            if (!_context.Categories.Any())
            {
                _context.Categories.AddRange(
                    new List<Category>
                    {
                        new Category { Name = "Clothing" },
                        new Category { Name = "Food" },
                        new Category { Name = "Medical Supplies" }
                    });
                _context.SaveChanges();
            }

            Categories = _context.Categories.ToList();

            // Handle tab query string (?tab=goods or ?tab=monetary)
            if (!string.IsNullOrEmpty(tab))
            {
                ActiveTab = tab.ToLower() == "goods" ? "goods" : "monetary";
            }
        }

        // -------------------------
        // MONETARY DONATION
        // -------------------------
        public async Task<IActionResult> OnPostMonetaryDonationAsync()
        {
            if (MonetaryForm.IsAnonymous)
            {
                ModelState.Remove("MonetaryForm.DonorName");
                ModelState.Remove("MonetaryForm.Email");
            }

            if (MonetaryForm.Amount <= 0)
            {
                ModelState.AddModelError("MonetaryForm.Amount", "Please enter a valid donation amount.");
            }

            if (!MonetaryForm.IsAnonymous)
            {
                if (string.IsNullOrWhiteSpace(MonetaryForm.DonorName))
                    ModelState.AddModelError("MonetaryForm.DonorName", "Name is required.");

                if (string.IsNullOrWhiteSpace(MonetaryForm.Email))
                    ModelState.AddModelError("MonetaryForm.Email", "Email is required.");
                else if (!System.Text.RegularExpressions.Regex.IsMatch(MonetaryForm.Email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$"))
                    ModelState.AddModelError("MonetaryForm.Email", "Please enter a valid email address.");
            }

            if (!ModelState.IsValid)
            {
                PopulateCategories();
                ActiveTab = "monetary";
                return Page();
            }

            try
            {
                var donor = await FindOrCreateDonorAsync(
                    MonetaryForm.IsAnonymous,
                    MonetaryForm.DonorName,
                    MonetaryForm.Email,
                    MonetaryForm.AnonymousId);

                var donation = new MonetaryDonation
                {
                    DonorName = donor.IsAnonymous ? "Anonymous" : donor.Name ?? "",
                    Amount = MonetaryForm.Amount,
                    DonorId = donor.Id,
                    DateDonated = DateTime.Now
                };

                _context.MonetaryDonations.Add(donation);
                await _context.SaveChangesAsync();

                // Set appropriate TempData based on donor type
                if (donor.IsAnonymous && !string.IsNullOrEmpty(donor.AnonymousId))
                {
                    TempData["MonetaryDonationSuccess"] = "anonymous";
                    TempData["AnonId"] = donor.AnonymousId;
                }
                else
                {
                    TempData["MonetaryDonationSuccess"] = "named";
                    TempData["DonorName"] = donor.Name;
                }

                return RedirectToPage("/Index");
            }
            catch
            {
                ModelState.AddModelError("", "An error occurred while processing your donation. Please try again.");
                PopulateCategories();
                ActiveTab = "monetary";
                return Page();
            }
        }

        // -------------------------
        // GOODS DONATION
        // -------------------------
        public async Task<IActionResult> OnPostGoodsDonationAsync()
        {
            if (GoodsForm.IsAnonymous)
            {
                ModelState.Remove("GoodsForm.DonorName");
                ModelState.Remove("GoodsForm.Email");
            }

            if (GoodsForm.CategoryId <= 0)
                ModelState.AddModelError("GoodsForm.CategoryId", "Please select a category.");

            if (GoodsForm.ItemsCount <= 0)
                ModelState.AddModelError("GoodsForm.ItemsCount", "Please enter the number of items.");

            if (string.IsNullOrWhiteSpace(GoodsForm.Description))
                ModelState.AddModelError("GoodsForm.Description", "Please provide a description.");

            // Validate drop-off scheduling
            if (GoodsForm.DropoffMethod == "Scheduled")
            {
                if (!GoodsForm.DropoffDateTime.HasValue)
                {
                    ModelState.AddModelError("GoodsForm.DropoffDateTime", "Please select a drop-off date and time.");
                }
                else if (GoodsForm.DropoffDateTime.Value < DateTime.Now)
                {
                    ModelState.AddModelError("GoodsForm.DropoffDateTime", "Drop-off time must be in the future.");
                }
            }

            if (!GoodsForm.IsAnonymous)
            {
                if (string.IsNullOrWhiteSpace(GoodsForm.DonorName))
                    ModelState.AddModelError("GoodsForm.DonorName", "Name is required.");

                if (string.IsNullOrWhiteSpace(GoodsForm.Email))
                    ModelState.AddModelError("GoodsForm.Email", "Email is required.");
                else if (!System.Text.RegularExpressions.Regex.IsMatch(GoodsForm.Email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$"))
                    ModelState.AddModelError("GoodsForm.Email", "Please enter a valid email address.");
            }

            if (!ModelState.IsValid)
            {
                PopulateCategories();
                ActiveTab = "goods";
                return Page();
            }

            try
            {
                var donor = await FindOrCreateDonorAsync(
                    GoodsForm.IsAnonymous,
                    GoodsForm.DonorName,
                    GoodsForm.Email,
                    GoodsForm.AnonymousId);

                // Generate unique reference number
                string referenceNumber = GenerateReferenceNumber();

                var goods = new GoodsDonation
                {
                    DonorName = donor.IsAnonymous ? "Anonymous" : donor.Name ?? "",
                    CategoryId = GoodsForm.CategoryId,
                    ItemsCount = GoodsForm.ItemsCount,
                    Description = GoodsForm.Description,
                    DonorId = donor.Id,
                    DateDonated = DateTime.Now,
                    ReferenceNumber = referenceNumber,
                    DropoffDateTime = GoodsForm.DropoffMethod == "Scheduled"
                        ? GoodsForm.DropoffDateTime
                        : DateTime.Now,
                    DropoffStatus = GoodsForm.DropoffMethod == "Scheduled" ? "Scheduled" : "Pending"
                };

                _context.GoodsDonations.Add(goods);
                await _context.SaveChangesAsync();

                // Store info for success message
                TempData["GoodsDonationSuccess"] = "true";
                TempData["ReferenceNumber"] = referenceNumber;
                TempData["DropoffMethod"] = GoodsForm.DropoffMethod;
                TempData["DropoffDateTime"] = GoodsForm.DropoffDateTime?.ToString("yyyy-MM-dd HH:mm");

                // Set donor info based on type
                if (donor.IsAnonymous && !string.IsNullOrEmpty(donor.AnonymousId))
                {
                    TempData["IsAnonymous"] = "true";
                    TempData["AnonId"] = donor.AnonymousId;
                }
                else
                {
                    TempData["IsAnonymous"] = "false";
                    TempData["DonorName"] = donor.Name;
                }

                return RedirectToPage("/Index");
            }
            catch
            {
                ModelState.AddModelError("", "An error occurred while processing your donation. Please try again.");
                PopulateCategories();
                ActiveTab = "goods";
                return Page();
            }
        }

        private string GenerateReferenceNumber()
        {
            // Format: GD-YYYYMMDD-XXXX (GD = Goods Donation)
            var datePart = DateTime.Now.ToString("yyyyMMdd");
            var random = new Random();
            var randomPart = random.Next(1000, 9999).ToString();
            return $"GD-{datePart}-{randomPart}";
        }
        // -------------------------
        // Helper
        // -------------------------
        private async Task<Donor> FindOrCreateDonorAsync(bool isAnon, string? name, string? email, string? anonId)
        {
            Donor? donor = null;

            if (isAnon)
            {
                if (!string.IsNullOrEmpty(anonId))
                    donor = _context.Donors.FirstOrDefault(d => d.AnonymousId == anonId);

                if (donor == null)
                {
                    donor = new Donor
                    {
                        Name = "Anonymous",
                        IsAnonymous = true,
                        AnonymousId = "ANON-" + Guid.NewGuid().ToString().Substring(0, 6).ToUpper()
                    };
                    _context.Donors.Add(donor);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                donor = _context.Donors.FirstOrDefault(d => d.Email == email && !d.IsAnonymous);
                if (donor == null)
                {
                    donor = new Donor
                    {
                        Name = name ?? "",
                        Email = email,
                        IsAnonymous = false
                    };
                    _context.Donors.Add(donor);
                    await _context.SaveChangesAsync();
                }
            }

            return donor;
        }
    }
}
