using System;
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
    public class MeetingsController : ApiController
    {
        private readonly IMeetingService _meetingService = new MeetingService();
        private readonly IRoomRepository _roomRepo = new RoomRepository();
        private HackDbContext db = new HackDbContext();

        [Route("api/meetings/start")]
        [HttpGet]
        public async Task<IHttpActionResult> Start(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();
            var s = id.Split('-');
            if (s.Length != 2)
                return BadRequest();

            var roomId = s[0];

            try
            {
                Trace.TraceInformation("[MeetingsController.GetStart] roomId=" + roomId);
                var room = await _roomRepo.GetRoomBySparkRoomID(roomId);
                if (room == null)
                    throw new ApplicationException("Can't start meeting for this room. Room is not found!");
                await _meetingService.StartMeetingAsync(room.Id);
                return Ok(Helpers.CreateExternalMessage());
            }
            catch (Exception ex)
            {
                var e = ex.GetInnerBottomException();
                Trace.TraceError(e.ToString());
                return ResponseMessage(Request.CreateResponse(
                    HttpStatusCode.InternalServerError, Helpers.CreateExternalMessage(e.Message)));
            }
        }

        [Route("api/meetings/manualstart")]
        [HttpGet]
        public async Task<IHttpActionResult> ManualStart(long roomId)
        {
            try
            {
                Trace.TraceInformation("[MeetingsController.GetMeetings] roomId=" + roomId);
                var meetService = new MeetingService();
                await meetService.StartMeetingAsync(roomId);
                return Ok("start meeting");
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.GetInnerBottomException().ToString());
                return Ok(ex.GetInnerBottomException().Message);
            }
        }        
    }
}