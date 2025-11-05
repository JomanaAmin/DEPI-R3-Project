using Bookify.DAL.Entities;

namespace Bookify.DAL.Repositories
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Cart?> GetCartWithItemsAsync(string customerId);
    }
}