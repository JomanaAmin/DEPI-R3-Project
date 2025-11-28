using System.ComponentModel.DataAnnotations;

namespace Bookify.MVC.Models
{
    public sealed class RoomUpdateRequest
    {
        [Required]
        public int Id { get; set; }

        [Required, StringLength(2)]
        public string Floor { get; set; } = string.Empty;

        [Required]
        public int RoomTypeId { get; set; }

        public bool IsAvailable { get; set; }

        [Range(0, 1000)]
        public double PricePerNight { get; set; }

        public List<string> ImageUrls { get; set; } = [];
    }
}