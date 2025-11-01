namespace DisasterAlleviation.Models.Forms
{
    public class MonetaryDonationForm
    {
        public bool IsAnonymous { get; set; }
        public string? DonorName { get; set; }
        public string? Email { get; set; }
        public string? AnonymousId { get; set; }
        public decimal Amount { get; set; }
    }
}
