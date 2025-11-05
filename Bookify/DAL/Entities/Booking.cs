using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bookify.DAL.Entities;

namespace Bookify.DAL.Entities
{
    public class Booking
    {
        public int BookingId { get; set; }

        [ForeignKey("Customer")]
        [Required]
        public string CustomerId { get; set; } = null!;
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
        public BookingStatus Status { get; set; } = BookingStatus.Pending;
        public decimal TotalAmount { get; set; }

        //Navigation Properties
        public CustomerProfile Customer { get; set; } = null!;

        //A Booking can exist before payment happens
        public Transaction? Transaction { get; set; }
        public ICollection<BookingItem> BookingItems { get; set; } = new HashSet<BookingItem>();

        //Helper method
        public void CalculateTotalAmount()
        {
            TotalAmount = BookingItems?.Sum(bi => bi.Subtotal)??0;
        }
    }

    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Cancelled
    }
}