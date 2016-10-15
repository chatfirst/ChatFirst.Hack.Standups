using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ChatFirst.Hack.Standups.Models;
using System.Threading.Tasks;

namespace ChatFirst.Hack.Standups.Controllers
{
    using Services;
    using Extensions;

    public class MeetingsController : ApiController
    {
        private HackDbContext db = new HackDbContext();

        // GET: api/Meetings/1
        public async Task<IHttpActionResult> GetMeetings(long roomId)
        {
            try
            {
                var meetService = new MeetingService();
                await meetService.StartMeetingAsync(roomId);
                return Ok("start meeting");
            }
            catch (Exception ex)
            {
                return Ok(ex.GetInnerBottomException().ToString());
            }
        }

        //// GET: api/Meetings/5
        //[ResponseType(typeof(Meeting))]
        //public IHttpActionResult GetMeeting(long id)
        //{
        //    Meeting meeting = db.Meetings.Find(id);
        //    if (meeting == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(meeting);
        //}

        //// PUT: api/Meetings/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutMeeting(long id, Meeting meeting)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != meeting.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(meeting).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!MeetingExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/Meetings
        //[ResponseType(typeof(Meeting))]
        //public IHttpActionResult PostMeeting(Meeting meeting)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Meetings.Add(meeting);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = meeting.Id }, meeting);
        //}

        //// DELETE: api/Meetings/5
        //[ResponseType(typeof(Meeting))]
        //public IHttpActionResult DeleteMeeting(long id)
        //{
        //    Meeting meeting = db.Meetings.Find(id);
        //    if (meeting == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Meetings.Remove(meeting);
        //    db.SaveChanges();

        //    return Ok(meeting);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private bool MeetingExists(long id)
        //{
        //    return db.Meetings.Count(e => e.Id == id) > 0;
        //}
    }
}