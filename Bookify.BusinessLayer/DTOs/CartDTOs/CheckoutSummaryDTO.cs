using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BusinessLayer.DTOs.CartDTOs
{
    public class CheckoutSummaryDTO
    {
        public int TotalItemsCount { get; set; }
        public decimal Total { get; set; }
        public string Currency { get; set; } = "EGP";
        public bool IsValid { get; set; }
    }
}
