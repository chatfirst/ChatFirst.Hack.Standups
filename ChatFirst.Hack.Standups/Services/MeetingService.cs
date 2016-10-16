using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using ChatFirst.Hack.Standups.Models;
using RestSharp;
using RestSharp.Authenticators;

namespace ChatFirst.Hack.Standups.Services
{
    public class MeetingService : IMeetingService
    {
        private readonly IConnectorClient _connectorClient = new ConnectorClient();
        private readonly IMetingAnswersRepository _metingAnswersRepository = new MetingAnswersRepository();
        private readonly IChatRoomUserService _userService = new ChatRoomUserService();
        private readonly IRoomRepository _roomRepository = new RoomRepository();

        public MeetingService()
        {
        }

        public MeetingService(IConnectorClient connectorClient, IChatRoomUserService chatRoomUserService,
            IMetingAnswersRepository metingAnswersRepository, IRoomRepository roomRepository)
        {
            _connectorClient = connectorClient;
            _userService = chatRoomUserService;
            _metingAnswersRepository = metingAnswersRepository;
            _roomRepository = roomRepository;
        }

        public async Task StartMeetingAsync(long roomId)
        {
            var meetId = await BuildMeetingAsync(roomId);
            var answer = await GetNextMeetingPushAsync(meetId);
            if (answer == null)
                return;

            var botName = await _roomRepository.GetBotName(roomId);
            var userId = $"{await _roomRepository.GetSparkRoomId(roomId)}-{answer.UserId}";

            await _connectorClient.PushRemoteChatService(botName, userId, answer.UserName);
        }

        public async Task<Answer> GetNextMeetingPushAsync(long meetId)
        {
            return await _metingAnswersRepository.GetNextMeetingPushAsync(meetId);
        }

        public async Task PushEndOfMeetingAsync(string botName, string roomId, string userId)
        {
            var urlService = string.Format(ConfigService.Get(Constants.UrlPushUserChatBot), botName,
                roomId + "-" + userId);
            var message = ConfigService.Get(Constants.TemplateMessage3);

            var restClient = new RestClient(urlService)
            {
                Authenticator = new HttpBasicAuthenticator(ConfigService.Get(Constants.UserToken),
                    string.Empty)
            };
            var req = new RestRequest("", Method.POST);
            req.AddJsonBody(new PushAnswerDataStruct
            {
                Count = 1,
                ForcedState = string.Empty,
                Messages = new List<string> {message}
            });
            var response = await restClient.ExecuteTaskAsync(req);
            Trace.TraceInformation("[MeetingService.PushEndOfMeeting] response: " + response.Content);
        }

        private async Task<long> BuildMeetingAsync(long roomId)
        {
            var room = await _metingAnswersRepository.FindRoom(roomId);
            var users = await _userService.GetUsersAsync(room.RoomId, room.BotName);
            var meet = await _metingAnswersRepository.SaveMeeting(room, users);
            return meet.Id;
        }
    }

    public  interface IRoomRepository
    {
        Task<string> GetSparkRoomId(long roomId);
        Task<string> GetBotName(long roomId);
    }

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

    internal class MetingAnswersRepository : IMetingAnswersRepository
    {
        public async Task<Meeting> SaveMeeting(Room room, IEnumerable<ChatRoomUser> users)
        {
            using (var db = new HackDbContext(ConfigService.Get(Constants.DbConnectionKey)))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var meet = new Meeting {DateStart = DateTime.UtcNow, RoomId = room.Id};

                        meet = db.Meetings.Add(meet);
                        await db.SaveChangesAsync();

                        var prepareAnswers = users.Select(u => new Answer
                        {
                            MeetingId = meet.Id,
                            UserId = u.userId,
                            UserName = u.userName.Split(' ').FirstOrDefault()
                        }).ToList();

                        db.Answers.AddRange(prepareAnswers);
                        await db.SaveChangesAsync();

                        transaction.Commit();                        

                        return meet;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task<Room> FindRoom(long roomId)
        {
            using (var db = new HackDbContext(ConfigService.Get(Constants.DbConnectionKey)))
            {
                var room = await db.Rooms.FirstOrDefaultAsync(r => r.Id == roomId);

                if (room == null)
                    throw new ApplicationException("Не существует roomId=" + roomId);
                var meetExist =
                    await db.Meetings.FirstOrDefaultAsync(m => m.RoomId == roomId && m.DateEnd == null);
                if (meetExist != null)
                    throw new ApplicationException("Митинг уже запущен в roomId=" + roomId);
                return room;
            }
        }

        public async Task<Answer> GetNextMeetingPushAsync(long meetId)
        {
            using (var db = new HackDbContext(ConfigService.Get(Constants.DbConnectionKey)))
            {
                return await db.Answers
                    .Where(a => a.MeetingId == meetId && a.Ans1 == null && a.Ans2 == null && a.Ans3 == null)
                    .OrderBy(a => a.Id)
                    .FirstOrDefaultAsync();
            }
        }
    }

    public interface IMetingAnswersRepository
    {
        Task<Answer> GetNextMeetingPushAsync(long meetId);
        Task<Room> FindRoom(long roomId);
        Task<Meeting> SaveMeeting(Room room, IEnumerable<ChatRoomUser> users);
    }

    internal class ConnectorClient : IConnectorClient
    {
        private readonly RestClient _restClient;

        public ConnectorClient()
        {
            _restClient = new RestClient("https://ch-core-mp.azurewebsites.net/v1/")
            {
                Authenticator = new HttpBasicAuthenticator(ConfigService.Get(Constants.UserToken), string.Empty)
            };
        }

        public async Task PushRemoteChatService(string botName, string userId, string userName)
        {
            //0 - userId, 1 - userName
            const string templatePerson = "<@personId:{0}|{1}>";
            var templateMsg1 = string.Format(ConfigService.Get(Constants.TemplateMessage1), templatePerson);
            var templateMsg2 = ConfigService.Get(Constants.TemplateMessage2);

            var req = new RestRequest("push/{botName}", Method.POST);
            req.AddUrlSegment("botName", botName);
            req.AddQueryParameter("id", userId);
            req.AddQueryParameter("channel", "spark");

            req.AddJsonBody(new PushAnswerDataStruct
            {
                Count = 1,
                ForcedState = ConfigService.Get(Constants.ForcedState),
                Messages =
                    new List<string> {string.Format(templateMsg1, userId, userName), templateMsg2}
            });
            var response = await _restClient.ExecuteTaskAsync(req);
            Trace.TraceInformation("[MeetingService.PushRemoteChatService] response: " + response.Content);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpException((int) HttpStatusCode.OK, "Failed to push answer", response.ErrorException);
        }
    }

    public interface IConnectorClient
    {
        Task PushRemoteChatService(string botName, string userId, string userName);
    }
}