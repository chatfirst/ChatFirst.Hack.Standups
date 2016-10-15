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

    public class MeetingService : IMeetingService
    {
        public async Task StartMeetingAsync(long roomId)
        {
            var meetId = await this.buildMeetingAsync(roomId);
            
        }

        public Task MeetingPush(long meetId)
        {
            return null;
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