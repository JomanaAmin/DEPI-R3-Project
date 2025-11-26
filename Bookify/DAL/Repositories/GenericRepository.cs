using Bookify.DAL.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bookify.DAL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly BookifyDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(BookifyDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<T?> GetByIdAsync(string id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        public IQueryable<T> GetAllAsQueryable() 
        {
            return _dbSet.AsQueryable();
        }

        public virtual async Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public virtual async Task<T?> Delete(int id)
        {
            T? toBeDeleted = await this.GetByIdAsync(id);
            if (toBeDeleted == null)
            {
                return null;
            }
            _dbSet.Remove(toBeDeleted);
            return toBeDeleted;
        }
        public virtual async Task<T?> Delete(string id)
        {
            T? toBeDeleted = await this.GetByIdAsync(id);
            if (toBeDeleted == null)
            {
                return null;
            }
            _dbSet.Remove(toBeDeleted);
            return toBeDeleted;
        }
        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}