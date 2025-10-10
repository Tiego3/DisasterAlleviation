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
    }
}
