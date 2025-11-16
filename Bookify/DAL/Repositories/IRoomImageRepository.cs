using Bookify.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.DAL.Repositories
{
    public interface IRoomImageRepository : IGenericRepository<RoomImage>
    {
        Task AddImagesRangeAsync(List<RoomImage> images);
        Task<List<RoomImage>> DeleteImagesRangeAsync(IEnumerable<int> ids);

    }
}
