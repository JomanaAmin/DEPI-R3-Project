namespace Bookify.MVC.Models.RoomTypeDTOs
{

    public class RoomTypeDetailsDTO
    {
        public int RoomTypeId { get; set; }
        public string TypeName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Capacity { get; set; }
        public decimal PricePerNight { get; set; }
        public int BedCount { get; set; }
        public string BedType { get; set; } = null!;
        public int BathroomCount { get; set; }
    }
}
