using System;
using System.Threading.Tasks;
using ChatFirst.Hack.Standups.Models;
using System.Data.Entity;

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

        public async Task<Room> GetRoomBySparkRoomID(string sparkRoomId)
        {
            using (var db = new HackDbContext())
            {
                return await db.Rooms.FirstOrDefaultAsync(r => r.RoomId == sparkRoomId);
            }
        }
    }
}