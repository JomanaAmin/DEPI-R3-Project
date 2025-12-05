
namespace Bookify.MVC.Models.BookingModels
{
    public class BookingViewDTO
    {
        public int BookingId { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
        public BookingStatus Status { get; set; } = BookingStatus.Pending;
        public decimal TotalAmount { get; set; }
        public List<BookingItemDTO> BookingItems { get; set; }
    }
}
