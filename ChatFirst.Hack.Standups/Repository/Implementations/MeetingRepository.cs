using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatFirst.Hack.Standups.Repository.Implementations
{
    using ModelViews;
    using Contract;
    using System.Threading.Tasks;

    public class MeetingRepository : BaseRepository, IMeetingRepository
    {
        public MeetingRepository() { }

        public MeetingRepository(string connStr) : base(connStr) { }

        public Task<IEnumerable<ViewMeeting>> MeetingAddRangeAsync(IEnumerable<ViewMeeting> meets)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ViewMeeting>> MeetingDeleteRangeByIdsAsync(IEnumerable<long> meetsId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ViewMeeting>> MeetingGetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ViewMeeting> MeetingGetByIdAsync(long meetId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ViewMeeting>> MeetingUpdateRangeAsync(IEnumerable<ViewMeeting> meets)
        {
            throw new NotImplementedException();
        }
    }
}