namespace Bookify.MVC.Models.RoomDTOs
{
    public class AllRoomsDTO
    {
        public List<RoomViewDTO>? Rooms { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
