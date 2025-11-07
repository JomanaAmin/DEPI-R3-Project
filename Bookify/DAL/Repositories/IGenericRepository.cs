using System.Linq.Expressions;

namespace Bookify.DAL.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<T?> GetByIdAsync(string id);
        Task<IEnumerable<T>> GetAllAsync();
        IQueryable<T> GetAllAsQueryable();
        Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> predicate);//An expression tree — EF Core can read and translate to SQL.
        Task CreateAsync(T entity);
        void Update(T entity);//deferred
        void Delete(T entity);//deferred
    }
}