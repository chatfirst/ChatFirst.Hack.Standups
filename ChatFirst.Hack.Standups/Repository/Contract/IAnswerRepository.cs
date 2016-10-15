using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatFirst.Hack.Standups.Repository.Contract
{
    using ModelViews;

    public interface IAnswerRepository
    {
        Task<IEnumerable<ViewAnswer>> AnswerAddRangeAsync(IEnumerable<ViewAnswer> answears);
        Task<IEnumerable<ViewAnswer>> AnswerDeleteRangeByIdsAsync(IEnumerable<long> answearIds);
        Task<IEnumerable<ViewAnswer>> AnswerUpdateRangeAsync(IEnumerable<ViewAnswer> answear);
        Task<ViewAnswer> AnswerGetByIdAsync(long answearId);
        Task<IEnumerable<ViewAnswer>> AnswerGetByMeetingIdAsync(long meetId);
        Task<IEnumerable<ViewAnswer>> AnswerGetAllAsync();

    }
}
