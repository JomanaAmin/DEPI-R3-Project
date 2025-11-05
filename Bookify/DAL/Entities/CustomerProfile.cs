using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bookify.DAL.Entities;

namespace Bookify.DAL.Entities
{
    public class CustomerProfile
    {
        [Key]
        [ForeignKey("User")]
        public string CustomerId { get; set; } = null!;

        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //Navigation Properties
        public BaseUser User { get; set; } = null!;
        public Cart Cart { get; set; } = null!;
        public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
        public ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();
    }
}