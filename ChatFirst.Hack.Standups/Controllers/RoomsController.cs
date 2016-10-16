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

namespace ChatFirst.Hack.Standups.Controllers
{
    using Services;
    using System.Threading.Tasks;
    public class RoomsController : ApiController
    {
        private HackDbContext db = new HackDbContext();
        private readonly IMeetingService _meetingService = new MeetingService();
        private readonly IRoomRepository _roomRepo = new RoomRepository();
        private readonly IMetingAnswersRepository _metingAnswersRepository = new MetingAnswersRepository();

        // GET: api/Rooms
        public IQueryable<Room> GetRooms()
        {
            return db.Rooms;
        }

        // GET: api/Rooms/5
        [ResponseType(typeof(Room))]
        public IHttpActionResult GetRoom(long id)
        {
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return NotFound();
            }

            return Ok(room);
        }

        // PUT: api/Rooms/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRoom(long id, Room room)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != room.Id)
            {
                return BadRequest();
            }

            db.Entry(room).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Rooms
        [ResponseType(typeof(Room))]
        public IHttpActionResult PostRoom(Room room)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Rooms.Add(room);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = room.Id }, room);
        }

        // DELETE: api/Rooms/5
        [ResponseType(typeof(Room))]
        public async Task<IHttpActionResult> DeleteRoom(long id)
        {
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return NotFound();
            }

            var meets = await _meetingService.GetMeetingsByRoomId(room.Id);
            
            foreach(var meet in meets)
            {
                await _metingAnswersRepository.DeleteAnswersInMiting(meet.Id);
            }

            var ids = meets.Select(m => m.Id).ToList();
            var toDelMeets = await db.Meetings.Where(m => ids.Contains(m.Id)).ToListAsync();
            db.Meetings.RemoveRange(toDelMeets);
            await db.SaveChangesAsync();
            db.Rooms.Remove(room);
            await db.SaveChangesAsync();

            return Ok(room);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RoomExists(long id)
        {
            return db.Rooms.Count(e => e.Id == id) > 0;
        }
    }
}