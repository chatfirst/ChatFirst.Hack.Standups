using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ChatFirst.Hack.Standups.Models
{
    using Services;

    public class HackDbContext : DbContext
    {
        public HackDbContext() : this(ConfigService.Get(Constants.DbConnectionKey) ?? "local") { }
        public HackDbContext(string nameConnection): base(nameConnection) { }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<Answer> Answers { get; set; }
    }
}