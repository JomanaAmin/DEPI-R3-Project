using Bookify.MVC.Models.RoomTypeDTOs;

namespace Bookify.MVC.Models.RoomDTOs
{
    public class RoomDetailsDTO
    {
        public int RoomId { get; set; }
        //public int RoomTypeId { get; set; }
        //public string RoomTypeName { get; set; }
        //public decimal PricePerNight { get; set; }
        public string Floor { get; set; } = null!;
        public string BuildingNumber { get; set; } = null!;
        public Status Status { get; set; } = Status.Available;
        public RoomTypeDetailsDTO RoomTypeDetails { get; set; }
        public List<string> Images { get; set; }
    }
}
