using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ChatFirst.Hack.Standups.Controllers
{
    using System.Diagnostics;
    using Extensions;
    using Models;
    using System.Data.Entity;
    using Services;

    public class AnswerbackController : ApiController
    {
        private IConnectorClient _connectorClient = new ConnectorClient();

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
                        var answer = await db.Answers.FirstOrDefaultAsync(a => a.MeetingId == meet.Id && a.UserId == userId);
                        if (answer == null)
                            return BadRequest($"userId={userId} not found in roomId={roomId}");
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
                                return BadRequest($"invalid question number={qnum}");
                        }
                        db.Entry(answer).State = EntityState.Modified;
                        await db.SaveChangesAsync();


                        if (string.IsNullOrEmpty(answer.Ans1)
                            || string.IsNullOrEmpty(answer.Ans2)
                            || string.IsNullOrEmpty(answer.Ans3))
                            //exist not answered
                            return Ok();
                        //init next push or end meeting
                        var meetSrv = new MeetingService();
                        var nextAnswer = await meetSrv.GetNextMeetingPushAsync(meet.Id);
                        if (nextAnswer == null)
                        {
                            // end meeting
                            //set endtime
                            meet.DateEnd = DateTime.Now;
                            db.Entry(meet).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                            //call remote api
                            await meetSrv.PushEndOfMeetingAsync(room.BotName, roomId, userId);
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
                            new PushAnswerDataStruct
                            {
                                Count = 1,
                                ForcedState = string.Empty,
                                //todo: set friendly messages
                                Messages = new List<string> { e.Message }
                            }
                        ));
                    }
                }
            }

            return Ok();
        }
    }
}
