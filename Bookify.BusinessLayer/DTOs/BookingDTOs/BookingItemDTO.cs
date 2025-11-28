using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BusinessLayer.DTOs.BookingDTOs
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
