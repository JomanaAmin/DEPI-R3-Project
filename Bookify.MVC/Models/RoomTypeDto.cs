namespace Bookify.MVC.Models
{
    public sealed class RoomTypeDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
        public double PricePerNight { get; init; }
        public int BedCount { get; set; }
        public string BedType { get; set; } = null!;
        public int BathroomCount { get; set; }
    }
}