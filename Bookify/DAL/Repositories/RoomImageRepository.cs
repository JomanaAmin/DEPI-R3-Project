using Bookify.DAL.Contexts;
using Bookify.DAL.Entities;
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
    }
}
