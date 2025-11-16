using Bookify.DAL.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BusinessLayer.DTOs.RoomDTOs
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
