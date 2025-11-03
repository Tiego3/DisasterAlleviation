using System;
using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviation.Models
{
    public class Volunteer
    {
        public int Id { get; set; }
        public string? UserId { get; set; }

        [Required] public string FullName { get; set; }
        [Required, EmailAddress] public string Email { get; set; }
        [Required] public string PhoneNumber { get; set; }
        [Required] public string Location { get; set; }
        [Required] public string Availability { get; set; }
        [Required] public string Skills { get; set; }

        public string? PreviousExperience { get; set; }

        [DataType(DataType.Date)]
        public DateTime? AvailableDates { get; set; }

        [Required] public string EmergencyContact { get; set; }

        public bool HasTransportation { get; set; }
        public bool WillingToTravel { get; set; }
        public string? AdditionalInfo { get; set; }

        // ✅ Admin-only / system fields
        [Required]
        public string Status { get; set; } = "Pending";

        public bool CanTravel { get; set; } = false;

        [Display(Name = "Date Applied")]
        public DateTime DateApplied { get; set; } = DateTime.UtcNow;
    }
}
