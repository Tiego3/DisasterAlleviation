using System;
using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviation.Models
{
    public class ResourceAllocation
    {
        public int Id { get; set; }

        [Required]
        public int DisasterId { get; set; }
        public Disaster? Disaster { get; set; }

        [Required]
        public string ResourceType { get; set; } // "Monetary" or "Goods"

        [Required]
        public string ResourceName { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        public string? Unit { get; set; }

        public string? SourceType { get; set; } // "Monetary" or "Goods"
        public int? SourceId { get; set; } // ID of source donation

        public string? Notes { get; set; }

        public DateTime AllocatedAt { get; set; } = DateTime.UtcNow;
    }
}