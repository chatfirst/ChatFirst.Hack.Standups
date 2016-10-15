using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatFirst.Hack.Standups.Services
{
    using Models;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using System.Diagnostics;
    using RestSharp;
    using RestSharp.Authenticators;
    public class MeetingService : IMeetingService
    {
        public async Task StartMeetingAsync(long roomId)
        {
            var meetId = await this.buildMeetingAsync(roomId);
            var answer = await this.GetNexMeetingPushAsync(meetId);
            if (answer == null)
                return;
            await this.InitingAnswerAsync(answer);
        }

        public async Task<Answer> GetNexMeetingPushAsync(long meetId)
        {
            using (var db = new HackDbContext(ConfigService.Get(Constants.DbConnectionKey)))
            {
                var meet = await db.Meetings.FirstOrDefaultAsync(m => m.Id == meetId);
                if (meet == null || meet.DateEnd != null)
                    return null;
                return await db.Answers
                        .Where(a => a.MeetingId == meetId && a.Ans1 == null && a.Ans2 == null && a.Ans3 == null)
                        .OrderBy(a => a.Id)
                        .FirstAsync();                 
            }
        }

        public async Task InitingAnswerAsync(Answer nextAnswer)
        {
            if (nextAnswer == null)
                return;
            
            using (var db = new HackDbContext(ConfigService.Get(Constants.DbConnectionKey)))
            {
                var meet = await db.Meetings.FirstOrDefaultAsync(m => m.Id == nextAnswer.MeetingId);
                if (meet == null)
                    return;
                var room = await db.Rooms.FirstOrDefaultAsync(r => r.Id == meet.RoomId);
                if (room == null)
                    return;
                var urlService = string.Format(ConfigService.Get(Constants.UrlPushUserChatBot), room.BotName,room.RoomId + "-" + nextAnswer.UserId);
                Trace.TraceInformation("[InitingAnswerAsync] url=" + urlService);
                await this.PushRemoteChatService(urlService, nextAnswer);
            }
        }

        public Task PushEndOfMeetingAsync(string botName, string roomId, string userId)
        {
            var urlService = string.Format(ConfigService.Get(Constants.UrlPushUserChatBot), botName, roomId + "-" + userId);
            var message = ConfigService.Get(Constants.TemplateMessage3);

            return Task.Run(() =>
            {
                var t = new TaskCompletionSource<object>();

                var restClient = new RestClient(urlService);
                restClient.Authenticator = new HttpBasicAuthenticator(ConfigService.Get(Constants.UserToken), string.Empty);
                var req = new RestRequest("", Method.POST);
                req.AddJsonBody(new PushAnswerDataStruct
                {
                    Count = 1,
                    ForcedState = string.Empty,
                    Messages = new List<string> { message }
                });
                restClient.ExecuteAsync(req, response => {
                    Trace.TraceInformation("[MeetingService.PushEndOfMeeting] response: " + response.Content);
                    t.TrySetResult(null);
                });

                return t.Task;
            });
        }

        private Task PushRemoteChatService(string url, Answer answer)
        {
            //0 - userId, 1 - userName
            var templatePerson = "<@personId:{0}|{1}>";
            var templateMsg1 = string.Format(ConfigService.Get(Constants.TemplateMessage1), templatePerson);
            var templateMsg2 = ConfigService.Get(Constants.TemplateMessage2);
            return Task.Run(() =>
            {
                var t = new TaskCompletionSource<object>();

                var restClient = new RestClient(url);
                restClient.Authenticator = new HttpBasicAuthenticator(ConfigService.Get(Constants.UserToken), string.Empty);
                var req = new RestRequest("", Method.POST);
                req.AddJsonBody(new PushAnswerDataStruct {
                    Count = 1,
                    ForcedState = ConfigService.Get(Constants.ForcedState),
                    Messages = new List<string> { string.Format(templateMsg1, answer.UserId, answer.UserName), templateMsg2 }
                });
                restClient.ExecuteAsync(req, response => {
                    Trace.TraceInformation("[MeetingService.PushRemoteChatService] response: " + response.Content);
                    t.TrySetResult(null);
                });

                return t.Task;
            });
        }

        private async Task<long> buildMeetingAsync(long roomId)
        {
            using (var db = new HackDbContext(ConfigService.Get(Constants.DbConnectionKey)))
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var room = await db.Rooms.FirstOrDefaultAsync(r => r.Id == roomId);

                        if (room == null)
                            throw new ApplicationException("Не существует roomId=" + roomId);
                        var meetExist = await db.Meetings.FirstOrDefaultAsync(m => m.RoomId == roomId && m.DateEnd == null);
                        if (meetExist != null)
                            throw new ApplicationException("Митинг уже запущен в roomId=" + roomId);

                        var userService = new ChatRoomUserService();

                        var users = await userService.GetUsersAsync(room.RoomId, room.BotName);

                        var meet = new Meeting { DateStart = DateTime.Now, RoomId = room.Id };

                        meet = db.Meetings.Add(meet);
                        await db.SaveChangesAsync();

                        var prepareAnswers = users.Select(u => new Answer
                        {
                            MeetingId = meet.Id,
                            UserId = u.userId,
                            UserName = u.userName.Split(' ')[0]
                        }).ToList();

                        prepareAnswers = db.Answers.AddRange(prepareAnswers).ToList();
                        await db.SaveChangesAsync();

                        transaction.Commit();

                        return meet.Id;
                    }
                    catch
                    {                        
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }

    public interface IMeetingService
    {
        Task StartMeetingAsync(long roomId);
    }

}