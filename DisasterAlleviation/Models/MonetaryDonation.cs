using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviation.Models
{
    public class MonetaryDonation
    {
        public int Id { get; set; }

        [Required]
        public string DonorName { get; set; } = string.Empty;

        [Required]
        [Range(1, double.MaxValue)]
        public decimal Amount { get; set; }
    }
}
