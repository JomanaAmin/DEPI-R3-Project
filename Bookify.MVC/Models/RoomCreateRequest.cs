using System.ComponentModel.DataAnnotations;

namespace Bookify.MVC.Models
{
    // Represents the payload to create a room from the MVC app to the API
    public sealed class RoomCreateRequest
    {
        [Required, StringLength(2)]
        public string Floor { get; set; } = string.Empty;

        [Required]
        public int RoomTypeId { get; set; }

        public bool IsAvailable { get; set; } = true;

        [Range(0, 1000)]
        public decimal PricePerNight { get; set; }

        // Accept URL/file uploads
        public List<string> ImageUrls { get; set; } = [];
    }
}