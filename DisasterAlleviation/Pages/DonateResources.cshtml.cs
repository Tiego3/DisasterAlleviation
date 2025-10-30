using DisasterAlleviation.Data;
using DisasterAlleviation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace DisasterAlleviation.Pages
{
    public class DonateResourcesModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DonateResourcesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Resource Type is required")]
            public string ResourceType { get; set; }
                        

            [Required(ErrorMessage = "NameOfItem is required")]
            public string NameOfItem { get; set; }

            [Required(ErrorMessage = "Date is required")]
            public DateTime Date { get; set; }

            [Required(ErrorMessage = "Number of Items is required")]
            [Range(1, int.MaxValue, ErrorMessage = "Must be at least 1 item")]
            public int NumberOfItems { get; set; }            

            [Required(ErrorMessage = "Description is required")]
            public string Description { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Create a new donation
            var donation = new Donation
            {
                ResourceType = Input.ResourceType,
                NameOfItem = Input.NameOfItem,
                Date = Input.Date,
                NumberOfItems = Input.NumberOfItems,               
                Description = Input.Description,
                Status = "Pending"
            };

            // Add donation to the database
            _context.Donations.Add(donation);
            await _context.SaveChangesAsync();
           
            return RedirectToPage("Dashboard");
        }
    }
}
