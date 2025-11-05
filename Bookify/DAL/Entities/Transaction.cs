using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bookify.DAL.Entities;

namespace Bookify.DAL.Entities
{
    public class Transaction
    {
        public int TransactionId { get; set; }

        [ForeignKey("Booking")]
        public int BookingId { get; set; }

        [ForeignKey("Customer")]
        [Required]
        public string CustomerId { get; set; } = null!;
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public decimal Amount { get; set; } //Persisted - copy from Booking
        public PaymentMethod PaymentMethod { get; set; }
        public TransactionStatus Status { get; set; } = TransactionStatus.Pending;

        //Navigation Properties
        public Booking Booking { get; set; } = null!;
        public CustomerProfile Customer { get; set; } = null!;
    }

    public enum PaymentMethod
    {
        CreditCard,
        DebitCard,
        Cash,
        Online
    }

    public enum TransactionStatus
    {
        Pending,
        Success,
        Failed
    }
}