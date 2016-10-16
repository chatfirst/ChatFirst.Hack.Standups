using System.Threading.Tasks;

namespace ChatFirst.Hack.Standups.Services
{
    public interface IMeetingService
    {
        Task StartMeetingAsync(long roomId);

        Task QuitMeetingAsync(string sparkRoomId);
    }
}