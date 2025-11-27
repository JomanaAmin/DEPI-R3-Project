using Bookify.BusinessLayer.DTOs.BookingDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BusinessLayer.Contracts
{
    public interface IBookingService
    {
        Task CreateBookingFromCartAsync(string customerId);
    }
}
