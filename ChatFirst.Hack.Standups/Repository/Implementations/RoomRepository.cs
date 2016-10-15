using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatFirst.Hack.Standups.Repository.Implementations
{
    using ModelViews;
    using Contract;
    using System.Threading.Tasks;

    public class RoomRepository : BaseRepository, IRoomRepository
    {
        public Task<IEnumerable<ViewRoom>> RoomAddRangeAsync(IEnumerable<ViewRoom> rooms)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ViewRoom>> RoomDeleteRangeByIdsAsync(IEnumerable<long> rooms)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ViewRoom>> RoomGetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ViewRoom> RoomGetByIdAsync(long roomId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ViewRoom>> RoomUpdateRangeAsync(IEnumerable<ViewAnswer> rooms)
        {
            throw new NotImplementedException();
        }
    }
}