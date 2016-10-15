using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChatFirst.Hack.Standups.Models
{
    /// <summary>
    /// Ответы юзера UserId митинга MeetingId 
    /// </summary>
    public class Answer
    {
        [Key]
        public long Id { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Ans1 { get; set; }
        public string Ans2 { get; set; }
        public string Ans3 { get; set; }

        public long? MeetingId { get; set; }
        public Meeting Meeting { get; set; }
    }
}