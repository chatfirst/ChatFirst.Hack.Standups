using System.Threading.Tasks;

namespace ChatFirst.Hack.Standups.Services
{
    using Models;

    public  interface IRoomRepository
    {
        Task<string> GetSparkRoomId(long roomId);
        Task<Room> GetRoomBySparkRoomID(string sparkRoomId);
        Task<string> GetBotName(long roomId);
    }
}