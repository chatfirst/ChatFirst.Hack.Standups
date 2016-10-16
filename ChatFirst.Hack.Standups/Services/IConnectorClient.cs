using System.Threading.Tasks;

namespace ChatFirst.Hack.Standups.Services
{
    public interface IConnectorClient
    {
        Task PushRemoteChatService(string botName, string userId, string userName);
        Task PushEndOfMeetingAsync(string botName, string userId);
    }
}