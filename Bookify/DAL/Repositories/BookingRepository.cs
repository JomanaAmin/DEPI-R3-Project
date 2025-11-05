using Bookify.DAL.Contexts;
using Bookify.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bookify.DAL.Repositories
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        public BookingRepository(BookifyDbContext context) : base(context)
        {
        }

        public async Task<Booking?> GetBookingWithDetailsAsync(int bookingId)
        {
            return await _dbSet
                .Include(b => b.Customer)
                .Include(b => b.BookingItems)
                    .ThenInclude(bi => bi.Room)
                        .ThenInclude(r => r.RoomType)
                .Include(b => b.Transaction)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);
        }

        public async Task<IEnumerable<Booking>> GetCustomerBookingsAsync(string customerId)
        {
            return await _dbSet
                .Include(b => b.BookingItems)
                    .ThenInclude(bi => bi.Room)
                        .ThenInclude(r => r.RoomType)
                .Where(b => b.CustomerId == customerId)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
        }
    }
}