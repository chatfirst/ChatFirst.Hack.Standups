using System.Threading.Tasks;
using ChatFirst.Hack.Standups.Models;

namespace ChatFirst.Hack.Standups.Services
{
    public class RoomRepository : IRoomRepository
    {
        public async Task<string> GetSparkRoomId(long roomId)
        {
            using (var db = new HackDbContext(ConfigService.Get(Constants.DbConnectionKey)))
            {
                var room = await db.Rooms.FindAsync(roomId);
                return room == null ? string.Empty : room.RoomId;
            }
        }

        public async Task<string> GetBotName(long roomId)
        {
            using (var db = new HackDbContext(ConfigService.Get(Constants.DbConnectionKey)))
            {
                var room = await db.Rooms.FindAsync(roomId);
                return room == null ? string.Empty : room.BotName;
            }
        }
    }
}