using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ChatFirst.Hack.Standups.Models;

namespace ChatFirst.Hack.Standups.Services
{
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
}