using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ChatFirst.Hack.Standups.Controllers
{
    using System.Diagnostics;
    using Extensions;
    using Services;
    using System.Threading.Tasks;
    public class ResultController : ApiController
    {
        private readonly IMetingAnswersRepository _metingAnswersRepository = new MetingAnswersRepository();
        private readonly IRoomRepository _roomRepository = new RoomRepository();

        [Route("api/result")]
        [HttpGet]
        public async Task<IHttpActionResult> Result(string id)
        {
            try
            {
                Trace.TraceInformation("[ResultController.Result] id=" + id);
                if (string.IsNullOrEmpty(id))
                    return BadRequest();
                var s = id.Split('-');
                if (s.Length != 2)
                    return BadRequest();

                var roomId = s[0];
                var room = await _roomRepository.GetRoomBySparkRoomID(roomId);
                if (room == null)
                    return BadRequest($"roomId={roomId} not found");
                var answers = await _metingAnswersRepository.GetLastMeetingAnswersByRoomId(room.Id);
                var extMsg = Helpers.CreateMarkdownResultAnswers(answers);
                Trace.TraceInformation(extMsg.Messages.ToString());
                return Ok(extMsg);
            }
            catch (Exception ex)
            {
                var e = ex.GetInnerBottomException();
                Trace.TraceError(e.ToString());
                return ResponseMessage(Request.CreateResponse(
                    HttpStatusCode.InternalServerError, Helpers.CreateExternalMessage(e.Message)));
            }
        }
    }
}
