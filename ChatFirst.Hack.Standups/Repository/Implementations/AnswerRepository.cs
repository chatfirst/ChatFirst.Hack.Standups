using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatFirst.Hack.Standups.Repository.Implementations
{
    using System.Threading.Tasks;
    using ModelViews;
    using Contract;

    public class AnswerRepository : BaseRepository, IAnswerRepository
    {
        public AnswerRepository() { }

        public AnswerRepository(string connStr) : base(connStr) { }

        public Task<IEnumerable<ViewAnswer>> AnswerAddRangeAsync(IEnumerable<ViewAnswer> answears)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ViewAnswer>> AnswerDeleteRangeByIdsAsync(IEnumerable<long> answearIds)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ViewAnswer>> AnswerGetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ViewAnswer> AnswerGetByIdAsync(long answearId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ViewAnswer>> AnswerGetByMeetingIdAsync(long meetId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ViewAnswer>> AnswerUpdateRangeAsync(IEnumerable<ViewAnswer> answear)
        {
            throw new NotImplementedException();
        }
    }
}