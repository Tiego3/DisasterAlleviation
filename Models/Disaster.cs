using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviation.Models
{
    public class Disaster
    {
        public int Id { get; set; }

        [Required]
        public string TypeOfDisaster { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public string? RequiredAidTypes { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Default status set to Active
        [Required]
        public string Status { get; set; } = "Active";

        // Navigation properties
        public List<Volunteer>? Volunteers { get; set; } = new();
       // public List<Resource>? Resources { get; set; } = new();
    }
}
