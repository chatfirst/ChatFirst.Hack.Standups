using System.Collections.Generic;
using System.Threading.Tasks;
using ChatFirst.Hack.Standups.Models;

namespace ChatFirst.Hack.Standups.Services
{
    public interface IMetingAnswersRepository
    {
        Task<Answer> GetNextMeetingPushAsync(long meetId);
        Task<Room> FindRoom(long roomId);
        Task<Meeting> SaveMeeting(Room room, IEnumerable<ChatRoomUser> users);
    }
}