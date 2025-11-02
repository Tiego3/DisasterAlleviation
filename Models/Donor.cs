using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviation.Models
{
    public class Donor
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string? Name { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }

        public bool IsAnonymous { get; set; }

        [MaxLength(50)]

        // Used for re-identifying anonymous donors
        public string? AnonymousId { get; set; } 

        public ICollection<MonetaryDonation>? MonetaryDonations { get; set; }
        public ICollection<GoodsDonation>? GoodsDonations { get; set; }
    }
}
