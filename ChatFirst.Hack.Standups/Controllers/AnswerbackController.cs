using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ChatFirst.Hack.Standups.Extensions;
using ChatFirst.Hack.Standups.Models;
using ChatFirst.Hack.Standups.Services;

namespace ChatFirst.Hack.Standups.Controllers
{
    public class AnswerbackController : ApiController
    {
        private readonly IConnectorClient _connectorClient = new ConnectorClient();
        private readonly IMetingAnswersRepository _metingAnswersRepository = new MetingAnswersRepository();

        [Route("api/skip")]
        public Task<IHttpActionResult> Skip(string id)
        {
            throw new NotImplementedException();
        }

        [Route("api/dismiss")]
        public Task<IHttpActionResult> Dismiss(string id)
        {
            throw new NotImplementedException();
        }


        [Route("api/answerback/{qnum}")]
        public async Task<IHttpActionResult> Get(int qnum, string id, string msg)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(msg))
                return BadRequest();
            var s = id.Split('-');
            if (s.Length != 2)
                return BadRequest();

            var roomId = s[0];
            var userId = s[1];

            using (var db = new HackDbContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var room = await db.Rooms.FirstOrDefaultAsync(r => r.RoomId == roomId);
                        if (room == null)
                            return BadRequest($"roomId={roomId} not found");
                        var meet = await db.Meetings.FirstOrDefaultAsync(m => m.RoomId == room.Id && m.DateEnd == null);
                        if (meet == null)
                            return BadRequest($"open meeting not found in roomId={roomId}");

                        var isNotComplete = await UpdateAnswer(qnum, msg, db, meet, userId, roomId);
                        if (isNotComplete)
                        {
                            transaction.Commit();
                            return Ok(CreateExternalMessage()); //exist not answered
                        }

                        //init next push or end meeting
                        var nextAnswer = await _metingAnswersRepository.GetNextMeetingPushAsync(meet.Id);
                        if (nextAnswer == null)
                        {
                            // end meeting
                            //set endtime
                            meet.DateEnd = DateTime.Now;
                            db.Entry(meet).State = EntityState.Modified;
                            await db.SaveChangesAsync();

                            //call remote api
                            await _connectorClient.PushEndOfMeetingAsync(room.BotName, $"{room.RoomId}-{userId}");
                        }
                        else
                        {
                            //nextAnswer.Meeting.Room.RoomId
                            await _connectorClient.PushRemoteChatService(room.BotName,
                                $"{room.RoomId}-{nextAnswer.UserId}", nextAnswer.UserName);
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        var e = ex.GetInnerBottomException();
                        Trace.TraceError(e.ToString());
                        return ResponseMessage(Request.CreateResponse(
                            HttpStatusCode.InternalServerError,
                            new ExternalMessage
                            {
                                Count = 1,
                                ForcedState = string.Empty,
                                //todo: set friendly messages
                                Messages = new List<string> {e.Message}
                            }
                            ));
                    }
                }
            }

            return Ok();
        }

        private static ExternalMessage CreateExternalMessage()
        {
            return new ExternalMessage {Count = 0, Messages = new List<string>()};
        }

        private async Task<bool> UpdateAnswer(int qnum, string msg, HackDbContext db, Meeting meet, string userId,
            string roomId)
        {
            var answer = await db.Answers.FirstOrDefaultAsync(a => a.MeetingId == meet.Id && a.UserId == userId);
            if (answer == null)
            {
                throw new Exception($"userId={userId} not found in roomId={roomId}");
            }
            switch (qnum)
            {
                case 1:
                    answer.Ans1 = msg;
                    break;
                case 2:
                    answer.Ans2 = msg;
                    break;
                case 3:
                    answer.Ans3 = msg;
                    break;
                default:
                    throw new ArgumentException($"invalid question number={qnum}");
            }
            db.Entry(answer).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return string.IsNullOrEmpty(answer.Ans1)
                   || string.IsNullOrEmpty(answer.Ans2)
                   || string.IsNullOrEmpty(answer.Ans3);
        }
    }
}