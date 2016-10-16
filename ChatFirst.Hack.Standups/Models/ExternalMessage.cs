using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatFirst.Hack.Standups.Models
{
    public class ExternalMessage
    {
        public int Count { get; set; }
        public List<string> Messages { get; set; }
        public string ForcedState { get; set; }
    }
}