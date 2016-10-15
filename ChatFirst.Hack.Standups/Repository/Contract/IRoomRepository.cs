using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatFirst.Hack.Standups.Repository.Contract
{
    using ModelViews;

    public interface IRoomRepository
    {
        Task<IEnumerable<ViewRoom>> RoomAddRangeAsync(IEnumerable<ViewRoom> rooms);
        Task<IEnumerable<ViewRoom>> RoomUpdateRangeAsync(IEnumerable<ViewAnswer> rooms);
        Task<IEnumerable<ViewRoom>> RoomDeleteRangeByIdsAsync(IEnumerable<long> rooms);
        Task<ViewRoom> RoomGetByIdAsync(long roomId);
        Task<IEnumerable<ViewRoom>> RoomGetAllAsync();
    }
}
