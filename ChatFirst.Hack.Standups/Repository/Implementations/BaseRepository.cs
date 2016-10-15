using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatFirst.Hack.Standups.Repository.Implementations
{
    using Models;

    public class BaseRepository
    {
        protected HackDbContext GetContext(string connStringName)
        {
            return new HackDbContext(connStringName);
        }

        protected HackDbContext GetContext()
        {
            return new HackDbContext();
        }
    }
}