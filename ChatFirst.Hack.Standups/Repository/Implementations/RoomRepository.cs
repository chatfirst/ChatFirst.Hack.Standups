using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatFirst.Hack.Standups.Repository.Implementations
{
    using ModelViews;
    using Contract;
    using System.Threading.Tasks;
    using Extensions;

    public class RoomRepository : BaseRepository, IRoomRepository
    {
        public RoomRepository() { }

        public RoomRepository(string connStr) : base(connStr) { }

        public async Task<IEnumerable<ViewRoom>> RoomAddRangeAsync(IEnumerable<ViewRoom> rooms)
        {
            if (rooms == null || !rooms.Any())
                return new List<ViewRoom>();
            using(var db = this.GetContext())
            {
                var models = rooms.Select(i => i.ViewRoomToModel());
                var dm = db.Rooms.AddRange(models);
                await db.SaveChangesAsync();
                return dm.Select(i => i.RoomToView());
            }
        }

        public async Task<IEnumerable<ViewRoom>> RoomDeleteRangeByIdsAsync(IEnumerable<long> rooms)
        {
            if (rooms == null || !rooms.Any())
                return new List<ViewRoom>();
            throw new NotImplementedException();
            //using (var db = this.GetContext())
            //{
            //    using (var trans = db.Database.BeginTransaction())
            //    {
            //        try
            //        {
                        
            //        }
            //        catch
            //        {
            //            trans.Rollback();
            //            throw;
            //        }
            //    }
            //}
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