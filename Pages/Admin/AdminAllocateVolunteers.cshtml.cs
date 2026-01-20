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
    public class AdminAllocateVolunteersModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AdminAllocateVolunteersModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public List<Disaster> ActiveDisasters { get; set; } = new();
        public List<Volunteer> ApprovedVolunteers { get; set; } = new();
        public int CanTravelCount { get; set; }
        public int HasTransportCount { get; set; }
        public int TotalAllocated { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Please select a disaster")]
            public int DisasterId { get; set; }

            [Required(ErrorMessage = "Please select a volunteer")]
            public int VolunteerId { get; set; }

            public string? AssignedRole { get; set; }
            public string? Notes { get; set; }
        }

        public async Task OnGetAsync()
        {
            // Load active disasters
            ActiveDisasters = await _context.Disasters
                .Where(d => d.Status == "Active")
                .OrderByDescending(d => d.StartDate)
                .ToListAsync();

            // Load approved volunteers who are not already allocated
            var allocatedVolunteerIds = await _context.Disasters
                .Where(d => d.Status == "Active")
                .SelectMany(d => d.Volunteers.Select(v => v.Id))
                .ToListAsync();

            ApprovedVolunteers = await _context.Volunteers
                .Where(v => v.Status == "Approved" && !allocatedVolunteerIds.Contains(v.Id))
                .OrderBy(v => v.FullName)
                .ToListAsync();

            // Calculate stats
            CanTravelCount = ApprovedVolunteers.Count(v => v.WillingToTravel);
            HasTransportCount = ApprovedVolunteers.Count(v => v.HasTransportation);
            TotalAllocated = allocatedVolunteerIds.Count;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            // Validate disaster exists and is active
            var disaster = await _context.Disasters
                .Include(d => d.Volunteers)
                .FirstOrDefaultAsync(d => d.Id == Input.DisasterId);

            if (disaster == null || disaster.Status != "Active")
            {
                return new JsonResult(new
                {
                    success = false,
                    message = "Disaster not found or is no longer active"
                })
                { StatusCode = 400 };
            }

            // Validate volunteer exists and is approved
            var volunteer = await _context.Volunteers
                .FirstOrDefaultAsync(v => v.Id == Input.VolunteerId);

            if (volunteer == null || volunteer.Status != "Approved")
            {
                return new JsonResult(new
                {
                    success = false,
                    message = "Volunteer not found or is not approved"
                })
                { StatusCode = 400 };
            }

            // Check if volunteer is already allocated to this disaster
            if (disaster.Volunteers.Any(v => v.Id == Input.VolunteerId))
            {
                return new JsonResult(new
                {
                    success = false,
                    message = "This volunteer is already allocated to this disaster"
                })
                { StatusCode = 400 };
            }

            // Allocate volunteer to disaster
            disaster.Volunteers.Add(volunteer);

            // Update volunteer with assignment details (optional: you can create a separate allocation table)
            if (!string.IsNullOrEmpty(Input.AssignedRole))
            {
                volunteer.AdditionalInfo = $"Assigned Role: {Input.AssignedRole}. {volunteer.AdditionalInfo}";
            }

            await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                success = true,
                redirectUrl = "/Admin/AdminDashboard"
            });
        }
    }
}