using Bookify.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BusinessLayer.DTOs.CartDTOs
{
    public class CartItemDTO
    {
        public int CartItemId { get; set; }

        public int CartId { get; set; }

        public int RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int Nights { get; set; }
        public string PreviewImageUrl { get; set; } = null!; 
        public decimal PricePerNight { get; set; }
        public decimal Subtotal { get; set; }
        
        //public string RoomTypeName { get; set; } = null!;

        //public bool IsAvailable { get; set; }
    }
}
