using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DisasterAlleviation.Data;
using DisasterAlleviation.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;

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
        }

        // -------------------------
        // MONETARY DONATION
        // -------------------------
        public async Task<IActionResult> OnPostMonetaryDonationAsync()
        {
            // Remove validation errors for fields that aren't required based on IsAnonymous
            if (MonetaryForm.IsAnonymous)
            {
                ModelState.Remove("MonetaryForm.DonorName");
                ModelState.Remove("MonetaryForm.Email");
            }

            // Validate required fields
            if (MonetaryForm.Amount <= 0)
            {
                ModelState.AddModelError("MonetaryForm.Amount", "Please enter a valid donation amount.");
            }

            // Validate name and email if not anonymous
            if (!MonetaryForm.IsAnonymous)
            {
                if (string.IsNullOrWhiteSpace(MonetaryForm.DonorName))
                {
                    ModelState.AddModelError("MonetaryForm.DonorName", "Name is required.");
                }

                if (string.IsNullOrWhiteSpace(MonetaryForm.Email))
                {
                    ModelState.AddModelError("MonetaryForm.Email", "Email is required.");
                }
                else
                {
                    var emailRegex = new System.Text.RegularExpressions.Regex(@"^[^\s@]+@[^\s@]+\.[^\s@]+$");
                    if (!emailRegex.IsMatch(MonetaryForm.Email))
                    {
                        ModelState.AddModelError("MonetaryForm.Email", "Please enter a valid email address.");
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                PopulateCategories();
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
                    DonorId = donor.Id
                };

                _context.MonetaryDonations.Add(donation);
                await _context.SaveChangesAsync();

                // If anonymous and new, show their ID after donation
                if (donor.IsAnonymous && !string.IsNullOrEmpty(donor.AnonymousId))
                {
                    TempData["AnonId"] = donor.AnonymousId;
                }

                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while processing your donation. Please try again.");
                PopulateCategories();
                return Page();
            }
        }

        // -------------------------
        // GOODS DONATION
        // -------------------------
        public async Task<IActionResult> OnPostGoodsDonationAsync()
        {
            // Remove validation errors for fields that aren't required based on IsAnonymous
            if (GoodsForm.IsAnonymous)
            {
                ModelState.Remove("GoodsForm.DonorName");
                ModelState.Remove("GoodsForm.Email");
            }

            // Validate required fields
            if (GoodsForm.CategoryId <= 0)
            {
                ModelState.AddModelError("GoodsForm.CategoryId", "Please select a category.");
            }

            if (GoodsForm.ItemsCount <= 0)
            {
                ModelState.AddModelError("GoodsForm.ItemsCount", "Please enter the number of items.");
            }

            if (string.IsNullOrWhiteSpace(GoodsForm.Description))
            {
                ModelState.AddModelError("GoodsForm.Description", "Please provide a description.");
            }

            // Validate name and email if not anonymous
            if (!GoodsForm.IsAnonymous)
            {
                if (string.IsNullOrWhiteSpace(GoodsForm.DonorName))
                {
                    ModelState.AddModelError("GoodsForm.DonorName", "Name is required.");
                }

                if (string.IsNullOrWhiteSpace(GoodsForm.Email))
                {
                    ModelState.AddModelError("GoodsForm.Email", "Email is required.");
                }
                else
                {
                    var emailRegex = new System.Text.RegularExpressions.Regex(@"^[^\s@]+@[^\s@]+\.[^\s@]+$");
                    if (!emailRegex.IsMatch(GoodsForm.Email))
                    {
                        ModelState.AddModelError("GoodsForm.Email", "Please enter a valid email address.");
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                PopulateCategories();
                return Page();
            }

            try
            {
                var donor = await FindOrCreateDonorAsync(
                    GoodsForm.IsAnonymous,
                    GoodsForm.DonorName,
                    GoodsForm.Email,
                    GoodsForm.AnonymousId);

                var goods = new GoodsDonation
                {
                    DonorName = donor.IsAnonymous ? "Anonymous" : donor.Name ?? "",
                    CategoryId = GoodsForm.CategoryId,
                    ItemsCount = GoodsForm.ItemsCount,
                    Description = GoodsForm.Description,
                    DonorId = donor.Id
                };

                _context.GoodsDonations.Add(goods);
                await _context.SaveChangesAsync();

                if (donor.IsAnonymous && !string.IsNullOrEmpty(donor.AnonymousId))
                {
                    TempData["AnonId"] = donor.AnonymousId;
                }

                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while processing your donation. Please try again.");
                PopulateCategories();
                return Page();
            }
        }

        // Helper method for both forms
        private async Task<Donor> FindOrCreateDonorAsync(bool isAnon, string? name, string? email, string? anonId)
        {
            Donor? donor = null;

            if (isAnon)
            {
                // Check if returning anonymous donor
                if (!string.IsNullOrEmpty(anonId))
                {
                    donor = _context.Donors.FirstOrDefault(d => d.AnonymousId == anonId);
                }

                // Create new anonymous donor if not found
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
                // Find or create named donor
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

    public class MonetaryDonationForm
    {
        public bool IsAnonymous { get; set; }
        public string? DonorName { get; set; }
        public string? Email { get; set; }
        public string? AnonymousId { get; set; }
        public decimal Amount { get; set; }
    }

    public class GoodsDonationForm
    {
        public bool IsAnonymous { get; set; }
        public string? DonorName { get; set; }
        public string? Email { get; set; }
        public string? AnonymousId { get; set; }
        public int CategoryId { get; set; }
        public int ItemsCount { get; set; }
        public string? Description { get; set; }
    }
}