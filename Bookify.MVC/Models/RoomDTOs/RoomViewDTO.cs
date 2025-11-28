using Bookify.DAL.Entities;

namespace Bookify.MVC.Models.RoomDTOs
{
    public class RoomViewDTO
    {
        public int RoomId { get; set; }
        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }

        public decimal PricePerNight { get; set; }

        public RoomStatus Status { get; set; } = RoomStatus.Available;
        public string ThumbnailImage { get; set; }
    }
}
