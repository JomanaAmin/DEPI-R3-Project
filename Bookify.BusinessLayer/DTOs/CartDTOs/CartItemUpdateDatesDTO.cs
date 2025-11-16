using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BusinessLayer.DTOs.CartDTOs
{
    public class CartItemUpdateDatesDTO
    {
        public int CartItemId { get; set; }
        public DateTime NewCheckInDate { get; set; }
        public DateTime NewCheckOutDate { get; set; }
    }
}
