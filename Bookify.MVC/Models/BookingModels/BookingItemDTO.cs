namespace Bookify.MVC.Models.BookingModels
{
    public class BookingItemDTO
    {
        public int BookingItemId { get; set; }
        public int BookingId { get; set; }
        public int RoomId { get; set; }
        public int Nights { get; set; }
        public decimal Subtotal { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

    }
}
