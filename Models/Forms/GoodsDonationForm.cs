namespace DisasterAlleviation.Models.Forms
{
    public class GoodsDonationForm
    {
        public bool IsAnonymous { get; set; }
        public string? DonorName { get; set; }
        public string? Email { get; set; }
        public string? AnonymousId { get; set; }
        public int CategoryId { get; set; }
        public int ItemsCount { get; set; }
        public string? Description { get; set; }
        public string DropoffMethod { get; set; } = "Scheduled"; // Scheduled or Immediate
        public DateTime? DropoffDateTime { get; set; }
    }
}
