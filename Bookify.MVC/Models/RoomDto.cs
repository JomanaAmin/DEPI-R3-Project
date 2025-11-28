namespace Bookify.MVC.Models
{
    public sealed class RoomDto
    {
        public int Id { get; init; }
        //public string description { get; init; } = string.Empty;
        public int RoomTypeId { get; init; }
        public string RoomTypeName { get; init; } = string.Empty;
        public bool IsAvailable { get; init; }
        public string Floor { get; set; } = null!;
        //
        public IReadOnlyList<string> ImageUrls { get; init; } = Array.Empty<string>();
    }
}