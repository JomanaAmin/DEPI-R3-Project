using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.DAL.Entities
{
    public class RoomImage
    {
        public int RoomImageId { get; set; }
        [ForeignKey("Room")]
        public int RoomId { get; set; }
        [Required]
        public string ImageUrl { get; set; } = null!;

    }
}
