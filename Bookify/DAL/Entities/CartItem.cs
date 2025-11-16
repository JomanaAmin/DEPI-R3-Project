using Bookify.DAL.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookify.DAL.Entities
{
    public class CartItem
    {
        public int CartItemId { get; set; }

        [ForeignKey("Cart")]
        public int CartId { get; set; }

        [ForeignKey("Room")]
        public int RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        
        //Computed property
        [NotMapped]
        public int Nights { get; set; }//i am gonna compute it in the service layer as (CheckOutDate - CheckInDate).Days

        [NotMapped]
        public decimal Subtotal { get; set; }// => (Room?.RoomType?.PricePerNight ?? 0) * Nights;
        //Navigation Properties
        public Cart Cart { get; set; } = null!;
        public Room Room { get; set; } = null!;
    }
}