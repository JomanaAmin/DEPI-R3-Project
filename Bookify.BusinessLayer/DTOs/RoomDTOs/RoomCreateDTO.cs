using Bookify.DAL.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BusinessLayer.DTOs.RoomDTOs
{
    public class RoomCreateDTO
    {

        public int RoomTypeId { get; set; }

        public string Floor { get; set; } = null!;

        public string BuildingNumber { get; set; } = null!;
        public RoomStatus Status { get; set; } = RoomStatus.Available;
        public List<IFormFile> Images { get; set; }
    }
}
