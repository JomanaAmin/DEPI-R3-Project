using Bookify.DAL.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookify.DAL.Entities
{
    public class Cart
    {
        public int CartId { get; set; }
        
        [ForeignKey("Customer")]
        [Required]
        public string CustomerId { get; set; } = null!;
        
        //Computed properties
        [NotMapped]
        public int NumberOfItems => CartItems?.Count ?? 0;

        //It is better to handle the computed properties in the service layer but it is lightweight so it is ok 
        [NotMapped]
        public decimal TotalAmount => CartItems?.Sum(ci => ci.Subtotal) ?? 0;

        //Navigation Properties
        public CustomerProfile Customer { get; set; } = null!;
        public ICollection<CartItem> CartItems { get; set; } = new HashSet<CartItem>();
    }
}