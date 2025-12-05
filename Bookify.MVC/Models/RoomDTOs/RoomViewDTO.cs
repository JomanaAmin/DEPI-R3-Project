
namespace Bookify.MVC.Models.RoomDTOs
{
    public class RoomViewDTO
    {
        public int RoomId { get; set; }
        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }

        public decimal PricePerNight { get; set; }

        public Status Status { get; set; } = Status.Available;
        public string ThumbnailImage { get; set; }
    }
}
