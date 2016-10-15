using System.Collections.Generic;
using System.Threading.Tasks;
using ChatFirst.Hack.Standups.Models;

namespace ChatFirst.Hack.Standups.Services
{
    public interface IChatRoomUserService
    {
        Task<List<ChatRoomUser>> GetUsersAsync(string roomId, string botName);
    }
}