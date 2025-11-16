using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BusinessLayer.DTOs.RoomTypeDTOs
{
    public class RoomTypeUpdateDTO
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
