using Bookify.DAL.Contexts;
using Bookify.DAL.Entities;
using Bookify.DAL.Repositories;

namespace Bookify.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BookifyDbContext _context;
        private Lazy<IRoomRepository> rooms;
        private Lazy<IBookingRepository> bookings;
        private Lazy<ICartRepository> carts;        
        private Lazy<IRoomImageRepository> roomImages;

        private Lazy<IGenericRepository<RoomType>> roomTypes;
        private Lazy<IGenericRepository<CartItem>> cartItems;
        private Lazy<IGenericRepository<BookingItem>> bookingItems;
        private Lazy<IGenericRepository<Transaction>> transactions;
        private Lazy<IGenericRepository<CustomerProfile>> customerProfiles;
        private Lazy<IGenericRepository<AdminProfile>> adminProfiles;
        public UnitOfWork(BookifyDbContext context)
        {
            _context = context;
            rooms = new Lazy<IRoomRepository>(() => new RoomRepository(_context));  
            bookings = new Lazy<IBookingRepository>(() => new BookingRepository(_context));
            carts = new Lazy<ICartRepository>(() => new CartRepository(_context));
            roomImages = new Lazy<IRoomImageRepository>(() => new RoomImageRepository(_context));
            roomTypes = new Lazy<IGenericRepository<RoomType>>(() => new GenericRepository<RoomType>(_context));
            cartItems = new Lazy<IGenericRepository<CartItem>>(() => new GenericRepository<CartItem>(_context));
            bookingItems = new Lazy<IGenericRepository<BookingItem>>(() => new GenericRepository<BookingItem>(_context));
            transactions = new Lazy<IGenericRepository<Transaction>>(() => new GenericRepository<Transaction>(_context));
            customerProfiles = new Lazy<IGenericRepository<CustomerProfile>>(() => new GenericRepository<CustomerProfile>(_context));
            adminProfiles = new Lazy<IGenericRepository<AdminProfile>>(() => new GenericRepository<AdminProfile>(_context));


        }
        public IRoomRepository Rooms => rooms.Value;
        public IBookingRepository Bookings => bookings.Value;
        public ICartRepository Carts =>  carts.Value;
        public IRoomImageRepository RoomImages => roomImages.Value;

        public IGenericRepository<RoomType> RoomTypes => roomTypes.Value;
        public IGenericRepository<CartItem> CartItems => cartItems.Value;
        public IGenericRepository<BookingItem> BookingItems => bookingItems.Value;
        public IGenericRepository<Transaction> Transactions => transactions.Value;
        public IGenericRepository<CustomerProfile> CustomerProfiles => customerProfiles.Value;
        public IGenericRepository<AdminProfile> AdminProfiles => adminProfiles.Value;

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