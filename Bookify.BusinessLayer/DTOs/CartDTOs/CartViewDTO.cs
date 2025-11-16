using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BusinessLayer.DTOs.CartDTOs
{
    public class CartViewDTO
    {
        public int CartId { get; set; }

        public List<CartItemDTO> Items { get; set; } = new List<CartItemDTO>();

        public decimal Total { get; set; }

        // Status Check
        public bool IsValid { get; set; }
        public string ValidationMessage { get; set; } = string.Empty;
    }
}
