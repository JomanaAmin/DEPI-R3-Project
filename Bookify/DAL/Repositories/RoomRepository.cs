using Bookify.DAL.Contexts;
using Bookify.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bookify.DAL.Repositories
{
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        public RoomRepository(BookifyDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Room>> GetAvailableRoomsAsync()
        {
            return await _dbSet
                .Include(r => r.RoomType)
                .Where(r => r.Status == RoomStatus.Available)
                .ToListAsync();
        }

        public async Task<Room?> GetRoomWithTypeAsync(int roomId)
        {
            return await _dbSet
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(r => r.RoomId == roomId);
        }

        public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut)
        {
            var room = await _dbSet.FindAsync(roomId);
            if (room == null || room.Status != RoomStatus.Available)
                return false;

            var hasConflict = await _context.BookingItems
                .AnyAsync(bi => bi.RoomId == roomId &&
                    bi.Booking.Status != BookingStatus.Cancelled &&
                    bi.CheckInDate < checkOut &&
                    bi.CheckOutDate > checkIn);

            return !hasConflict;
        }
    }
}