using DisasterAlleviation.Data;
using DisasterAlleviation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DisasterAlleviation.Pages
{
    [Authorize(Roles = "Admin")]
    public class AdminViewDonationsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public List<MonetaryDonation> MonetaryDonations { get; set; } = new();
        public List<GoodsDonation> GoodsDonations { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; } = "date";

        [BindProperty(SupportsGet = true)]
        public string SortOrder { get; set; } = "desc";

        [BindProperty(SupportsGet = true)]
        public DateTime? FromDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? ToDate { get; set; }

        public AdminViewDonationsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            var monetaryQuery = _context.MonetaryDonations
                .Include(d => d.Donor)
                .AsQueryable();

            var goodsQuery = _context.GoodsDonations
                .Include(d => d.Donor)
                .Include(d => d.Category)
                .AsQueryable();

            // 🔍 Filter by search
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                monetaryQuery = monetaryQuery.Where(d => d.DonorName.Contains(SearchTerm));
                goodsQuery = goodsQuery.Where(d => d.DonorName.Contains(SearchTerm));
            }

            // 📅 Filter by date (if you have a CreatedAt or DateDonated property, replace d.Id filter)
            // For now, we assume the Id roughly represents chronological order if no date column exists
            if (FromDate.HasValue && ToDate.HasValue)
            {
                // Replace with donation.DateDonated if available
                // This is a placeholder filter
            }

            // ↕ Sorting
            bool descending = SortOrder == "desc";

            switch (SortBy)
            {
                case "amount":
                    monetaryQuery = descending
                        ? monetaryQuery.OrderByDescending(d => d.Amount)
                        : monetaryQuery.OrderBy(d => d.Amount);
                    break;
                case "donor":
                    monetaryQuery = descending
                        ? monetaryQuery.OrderByDescending(d => d.DonorName)
                        : monetaryQuery.OrderBy(d => d.DonorName);
                    goodsQuery = descending
                        ? goodsQuery.OrderByDescending(d => d.DonorName)
                        : goodsQuery.OrderBy(d => d.DonorName);
                    break;
                default: // "date"
                    monetaryQuery = descending
                        ? monetaryQuery.OrderByDescending(d => d.Id)
                        : monetaryQuery.OrderBy(d => d.Id);
                    goodsQuery = descending
                        ? goodsQuery.OrderByDescending(d => d.Id)
                        : goodsQuery.OrderBy(d => d.Id);
                    break;
            }

            MonetaryDonations = monetaryQuery.ToList();
            GoodsDonations = goodsQuery.ToList();
        }
    }
}
