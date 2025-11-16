using Bookify.DAL.Contexts;
using Bookify.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.DAL.Repositories
{
    internal class RoomImageRepository : GenericRepository<RoomImage>, IRoomImageRepository
    {
        public RoomImageRepository(BookifyDbContext context) : base(context)
        {

        }            
        public async Task AddImagesRangeAsync(List<RoomImage> images)
        {
            await _dbSet.AddRangeAsync(images);
        }
        public async Task<List<RoomImage>> DeleteImagesRangeAsync(IEnumerable<int> ids)
        {
            var toBeDeleted = await _dbSet.Where(img => ids.Contains(img.RoomImageId)).ToListAsync();
            if (toBeDeleted.Any())
            {
                _dbSet.RemoveRange(toBeDeleted);
            }
            return toBeDeleted;
        }

    }
}
