using Bookify.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookify.DAL.Entities
{
    public class BookingItem
    {
        public int BookingItemId { get; set; }

        [ForeignKey("Booking")]
        public int BookingId { get; set; }

        [ForeignKey("Room")]
        public int RoomId { get; set; }
        public int Nights { get; set; }
        public decimal Subtotal { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

        //Navigation Properties
        public Booking Booking { get; set; } = null!;
        public Room Room { get; set; } = null!;

        //helper method to calculate (called before saving)
        public void CalculateSubtotal()
        {
            Nights = (CheckOutDate - CheckInDate).Days;
            Subtotal = (Room?.RoomType?.PricePerNight ?? 0) * Nights;
        }

    }
}