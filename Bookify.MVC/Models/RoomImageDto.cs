namespace Bookify.MVC.Models
{
    public sealed class RoomImageDto
    {
        public int Id { get; init; }
        public int RoomId { get; init; }
        public string Url { get; init; } = string.Empty;
       // public string? Caption { get; init; }
    }
}