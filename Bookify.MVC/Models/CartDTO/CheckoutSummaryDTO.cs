

namespace Bookify.MVC.Models.CartDTO
{
    public class CheckoutSummaryDTO
    {
        public int TotalItemsCount { get; set; }
        public decimal Total { get; set; }
        public string Currency { get; set; } = "EGP";
        public bool IsValid { get; set; }
    }
}
