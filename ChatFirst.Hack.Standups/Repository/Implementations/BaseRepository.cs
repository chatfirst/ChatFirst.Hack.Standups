using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatFirst.Hack.Standups.Repository.Implementations
{
    using Models;

    public class BaseRepository
    {
        protected string ConnectionString { get; private set; }
        protected HackDbContext GetContext(string connStringName)
        {
            return new HackDbContext(connStringName);
        }

        protected HackDbContext GetContext()
        {
            return this.ConnectionString == null ? new HackDbContext() : new HackDbContext(this.ConnectionString);
        }

        public BaseRepository() { }

        public BaseRepository(string connString)
        {
            this.ConnectionString = connString;
        }
    }
}