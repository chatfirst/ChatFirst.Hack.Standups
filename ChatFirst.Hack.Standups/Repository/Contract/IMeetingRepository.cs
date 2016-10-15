using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatFirst.Hack.Standups.Repository.Contract
{
    using ModelViews;

    public interface IMeetingRepository
    {
        Task<IEnumerable<ViewMeeting>> MeetingAddRangeAsync(IEnumerable<ViewMeeting> meets);
        Task<IEnumerable<ViewMeeting>> MeetingUpdateRangeAsync(IEnumerable<ViewMeeting> meets);
        Task<IEnumerable<ViewMeeting>> MeetingDeleteRangeByIdsAsync(IEnumerable<long> meetsId);
        Task<ViewMeeting> MeetingGetByIdAsync(long meetId);
        Task<IEnumerable<ViewMeeting>> MeetingGetAllAsync();
    }
}
