using Bookify.BusinessLayer.DTOs.RoomTypeDTOs;
using Bookify.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BusinessLayer.DTOs.RoomDTOs
{
    public class RoomDetailsDTO
    {
        public int RoomId { get; set; }
        //public int RoomTypeId { get; set; }
        //public string RoomTypeName { get; set; }
        //public decimal PricePerNight { get; set; }
        public string Floor { get; set; } = null!;
        public string BuildingNumber { get; set; } = null!;
        public RoomStatus Status { get; set; } = RoomStatus.Available;
        public RoomTypeDetailsDTO RoomTypeDetails { get; set; }
        public List<string> Images { get; set; }
    }
}
