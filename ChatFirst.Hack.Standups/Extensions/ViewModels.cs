using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatFirst.Hack.Standups.Extensions
{
    using Models;
    using ModelViews;

    public static class ViewModels
    {
        public static Room ViewRoomToModel(this ViewRoom vr)
        {
            return new Room
            {
                Id = vr.Id,
                RoomId = vr.RoomId,
                TeamId = vr.TeamId,
                Cron = vr.Cron
            };
        }

        public static ViewRoom RoomToView(this Room r)
        {
            var room = new ViewRoom
            {
                Id = r.Id,
                RoomId = r.RoomId,
                TeamId = r.TeamId,
                Cron = r.Cron
            };

            room.Meetings = r.Meetings?.Select(i => i.MeetingToView()).ToList();
            return room;
        }

        public static ViewMeeting MeetingToView(this Meeting meet)
        {
            var vm = new ViewMeeting
            {
                Id = meet.Id,
                DateStart = meet.DateStart,
                DateEnd = meet.DateEnd,
                RoomId = meet.RoomId
            };
            vm.Answers = meet.Answers?.Select(i => i.AnswerToView()).ToList();
            return vm;
        }

        public static Meeting ViewMeetingToModel(this ViewMeeting meet)
        {
            return new Meeting
            {
                Id = meet.Id,
                DateStart = meet.DateStart,
                DateEnd = meet.DateEnd,
                RoomId = meet.RoomId
            };
        }

        public static ViewAnswer AnswerToView(this Answer ans)
        {
            return new ViewAnswer
            {
                Id = ans.Id,
                UserName = ans.UserName,
                UserId = ans.UserId,
                MeetingId = ans.MeetingId,
                Ans1 = ans.Ans1,
                Ans2 = ans.Ans2,
                Ans3 = ans.Ans3
            };
        }
    }
}