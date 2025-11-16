using Bookify.DAL.Entities;
using Bookify.DAL.Repositories;

namespace Bookify.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        //Specific Repositories
        IRoomRepository Rooms { get; }
        IBookingRepository Bookings { get; }
        ICartRepository Carts { get; }
        IRoomImageRepository RoomImages { get; }

        //Generic Repositories
        IGenericRepository<RoomType> RoomTypes { get; }
        IGenericRepository<CartItem> CartItems { get; }
        IGenericRepository<BookingItem> BookingItems { get; }
        IGenericRepository<Transaction> Transactions { get; }
        IGenericRepository<CustomerProfile> CustomerProfiles { get; }
        IGenericRepository<AdminProfile> AdminProfiles { get; }

        Task<int> SaveChangesAsync();
    }
}