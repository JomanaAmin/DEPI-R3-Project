using System.ComponentModel.DataAnnotations;

namespace Bookify.DAL.Entities
{
    public class RoomType
    {
        public int RoomTypeId { get; set; }

        [Required]
        public string TypeName { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;
        public int Capacity { get; set; }
        public decimal PricePerNight { get; set; }
        public int BedCount { get; set; }
        [Required]
        public string BedType { get; set; } = null!;
        public int BathroomCount { get; set; }

        //Navigation Properties
        public ICollection<Room> Rooms { get; set; } = new HashSet<Room>();

    }

}