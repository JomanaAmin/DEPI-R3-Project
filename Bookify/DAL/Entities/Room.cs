using Bookify.DAL.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookify.DAL.Entities
{
    public class Room
    {
        public int RoomId { get; set; }

        [ForeignKey("RoomType")]
        public int RoomTypeId { get; set; }

        [Required]
        public string Floor { get; set; } = null!;

        [Required]
        public string BuildingNumber { get; set; } = null!;
        public RoomStatus Status { get; set; } =RoomStatus.Available;

        //Navigation Properties
        public RoomType RoomType { get; set; } = null!;
        public ICollection<BookingItem> BookingItems { get; set; } = new HashSet<BookingItem>();
        public ICollection<CartItem> CartItems { get; set; } = new HashSet<CartItem>();
        public ICollection<RoomImage> RoomImages { get; set; }
    }

    public enum RoomStatus
    {
        Available,
        Occupied,
        Reserved,
        Maintenance
    }

}
