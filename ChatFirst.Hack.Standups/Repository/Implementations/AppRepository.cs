using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatFirst.Hack.Standups.Repository.Implementations
{
    using ModelViews;
    using Contract;
    using System.Threading.Tasks;

    public class AppRepository : IAppRepository
    {
        IAnswerRepository _answerRepo;
        IRoomRepository _roomRepo;
        IMeetingRepository _meetRepo;

        public AppRepository(
                IAnswerRepository ansRepo,
                IRoomRepository roomRepo,
                IMeetingRepository meetRepo
            )
        {
            this._answerRepo = ansRepo;
            this._meetRepo = meetRepo;
            this._roomRepo = roomRepo;
        }

        public AppRepository() : this(
                new AnswerRepository(),
                new RoomRepository(),
                new MeetingRepository()
            ) { }

        public AppRepository(string connStr) : this(
                new AnswerRepository(connStr),
                new RoomRepository(connStr),
                new MeetingRepository(connStr)
            )
        { }

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