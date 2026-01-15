namespace DisasterAlleviation.Models
{
    public class GoodsDropoffViewModel
    {
        public int Id { get; set; }
        public string ReferenceNumber { get; set; }
        public string DonorName { get; set; }
        public string ContactInfo { get; set; }
        public string CategoryName { get; set; }
        public int ItemsCount { get; set; }
        public string Description { get; set; }
        public DateTime? DropoffDateTime { get; set; }
        public string DropoffStatus { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? AdminNotes { get; set; }
    }
}
