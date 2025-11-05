using Bookify.DAL.Contexts;
using Bookify.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bookify.DAL.Repositories
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(BookifyDbContext context) : base(context)
        {
        }

        public async Task<Cart?> GetCartWithItemsAsync(string customerId)
        {
            return await _dbSet
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Room)
                        .ThenInclude(r => r.RoomType)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }
    }
}
