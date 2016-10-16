using System.Threading.Tasks;

namespace ChatFirst.Hack.Standups.Services
{
    public  interface IRoomRepository
    {
        Task<string> GetSparkRoomId(long roomId);
        Task<string> GetBotName(long roomId);
    }
}