using Bookify.DAL.Entities;

namespace Bookify.DAL.Repositories
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task<Booking?> GetBookingWithDetailsAsync(int bookingId);
        Task<IEnumerable<Booking>> GetCustomerBookingsAsync(string customerId);
    }
}