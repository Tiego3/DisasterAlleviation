using DisasterAlleviation.Pages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DisasterAlleviation.Models
{
    public class GoodsDonation
    {
        public int Id { get; set; }

        [Required]
        public string DonorName { get; set; } = string.Empty;

        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int ItemsCount { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;
        public int DonorId { get; set; } 
        public Donor Donor { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateDonated { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime? DropoffDateTime { get; set; }

        [MaxLength(20)]
        public string? ReferenceNumber { get; set; }

        [MaxLength(50)]
        public string DropoffStatus { get; set; } = "Pending"; // Pending, Scheduled, Completed, Cancelled

        public DateTime? CompletedAt { get; set; }

        [MaxLength(500)]
        public string? AdminNotes { get; set; }
    }
}
