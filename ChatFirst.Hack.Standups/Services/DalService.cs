using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatFirst.Hack.Standups.Services
{
    using Repository;
    using Services;

    public class DalService
    {
        public string ConnectionString { get; private set; }

        public DalService(string connStr)
        {
            this.ConnectionString = connStr;
        }

        public DalService(): this(ConfigService.Get(Constants.DbConnectionKey)) { }
    }
}