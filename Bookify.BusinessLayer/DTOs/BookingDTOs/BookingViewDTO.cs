using Bookify.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BusinessLayer.DTOs.BookingDTOs
{
    public class BookingViewDTO
    {
        public int BookingId {get; set;}
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
        public BookingStatus Status { get; set; } = BookingStatus.Pending;
        public decimal TotalAmount { get; set; }
        public List<BookingItem> BookingItems { get; set; }

    }
}
