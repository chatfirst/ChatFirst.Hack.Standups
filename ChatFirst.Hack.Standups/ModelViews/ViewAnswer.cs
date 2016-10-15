using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatFirst.Hack.Standups.ModelViews
{
    public class ViewAnswer
    {
        public long Id { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Ans1 { get; set; }
        public string Ans2 { get; set; }
        public string Ans3 { get; set; }

        public long? MeetingId { get; set; }
    }
}