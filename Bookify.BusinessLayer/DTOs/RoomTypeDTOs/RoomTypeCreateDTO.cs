using Bookify.DAL.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BusinessLayer.DTOs.RoomTypeDTOs
{
    public class RoomTypeCreateDTO
    {
        public string TypeName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Capacity { get; set; }
        public decimal PricePerNight { get; set; }
        public int BedCount { get; set; }
        public string BedType { get; set; } = null!;
        public int BathroomCount { get; set; }
        
    }
}
