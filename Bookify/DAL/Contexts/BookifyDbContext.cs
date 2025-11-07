using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Bookify.DAL.Entities;
using Microsoft.EntityFrameworkCore.Design;

namespace Bookify.DAL.Contexts
{
    public class BookifyDbContext : IdentityDbContext<BaseUser>
    {
        public BookifyDbContext(DbContextOptions<BookifyDbContext> options)
            : base(options)
        {
        }
        public DbSet<AdminProfile> AdminProfiles { get; set; }
        public DbSet<CustomerProfile> CustomerProfiles { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingItem> BookingItems { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BaseUser>(entity =>
            {
                entity.HasOne(u => u.CustomerProfile)
                    .WithOne(c => c.User)
                    .HasForeignKey<CustomerProfile>(c => c.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(u => u.AdminProfile)
                    .WithOne(a => a.User)
                    .HasForeignKey<AdminProfile>(a => a.AdminId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CustomerProfile>(entity =>
            {
                entity.HasMany(c => c.Bookings)
                    .WithOne(b => b.Customer)
                    .HasForeignKey(b => b.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(c => c.Transactions)
                    .WithOne(t => t.Customer)
                    .HasForeignKey(t => t.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<RoomType>(entity =>
            {
                entity.Property(rt => rt.PricePerNight)
                    .HasPrecision(18, 2);

                entity.HasMany(rt => rt.Rooms)
                    .WithOne(r => r.RoomType)
                    .HasForeignKey(r => r.RoomTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasMany(r => r.BookingItems)
                    .WithOne(bi => bi.Room)
                    .HasForeignKey(bi => bi.RoomId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(r => r.CartItems)
                    .WithOne(ci => ci.Room)
                    .HasForeignKey(ci => ci.RoomId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(r => r.RoomImages)
                    .WithOne(ri => ri.Room)
                    .HasForeignKey(ri => ri.RoomId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Booking>(entity =>
            {

                entity.Property(b => b.TotalAmount)
                    .HasPrecision(18, 2);

                entity.HasOne(b => b.Transaction)
                    .WithOne(t => t.Booking)
                    .HasForeignKey<Transaction>(t => t.BookingId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<BookingItem>(entity =>
            {
                entity.Property(bi => bi.Subtotal)
                    .HasPrecision(18, 2);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(t => t.Amount)
                    .HasPrecision(18, 2);
            });

        }
    }

namespace Bookify.DAL.Contexts
    {
        public class BookifyDbContextFactory : IDesignTimeDbContextFactory<BookifyDbContext>
        {
            public BookifyDbContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<BookifyDbContext>();

                
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=BookifyDb;Integrated Security=True;Trust Server Certificate=True");

                return new BookifyDbContext(optionsBuilder.Options);
            }
        }
    }

}