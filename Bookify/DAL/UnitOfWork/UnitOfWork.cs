using Bookify.DAL.Contexts;
using Bookify.DAL.Entities;
using Bookify.DAL.Repositories;

namespace Bookify.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BookifyDbContext _context;

        public UnitOfWork(BookifyDbContext context)
        {
            _context = context;

            Rooms = new RoomRepository(_context);
            Bookings = new BookingRepository(_context);
            Carts = new CartRepository(_context);

            RoomTypes = new GenericRepository<RoomType>(_context);
            RoomImages = new GenericRepository<RoomImage>(_context);
            CartItems = new GenericRepository<CartItem>(_context);
            BookingItems = new GenericRepository<BookingItem>(_context);
            Transactions = new GenericRepository<Transaction>(_context);
            CustomerProfiles = new GenericRepository<CustomerProfile>(_context);
            AdminProfiles = new GenericRepository<AdminProfile>(_context);
        }    
        public IRoomRepository Rooms { get; }
        public IBookingRepository Bookings { get; }
        public ICartRepository Carts { get; }
        public IGenericRepository<RoomType> RoomTypes { get; }
        public IGenericRepository<RoomImage> RoomImages { get; }
        public IGenericRepository<CartItem> CartItems { get; }
        public IGenericRepository<BookingItem> BookingItems { get; }
        public IGenericRepository<Transaction> Transactions { get; }
        public IGenericRepository<CustomerProfile> CustomerProfiles { get; }
        public IGenericRepository<AdminProfile> AdminProfiles { get; }

        public async Task<int> SaveChangesAsync()//returns the number of affected rows
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}