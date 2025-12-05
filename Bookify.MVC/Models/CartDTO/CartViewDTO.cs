

namespace Bookify.MVC.Models.CartDTO
{
    public class CartViewDTO
    {
        public int CartId { get; set; }

        public List<CartItemDTO> Items { get; set; } = new List<CartItemDTO>();

        public decimal Total { get; set; }

        // Status Check
        public bool IsValid { get; set; }
        public string ValidationMessage { get; set; } = string.Empty;
    }
}
