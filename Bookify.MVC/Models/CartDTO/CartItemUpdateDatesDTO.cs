namespace Bookify.MVC.Models.CartDTO
{
    public class CartItemUpdateDatesDTO
    {
        public int CartItemId { get; set; }
        public DateTime NewCheckInDate { get; set; }
        public DateTime NewCheckOutDate { get; set; }
    }
}
