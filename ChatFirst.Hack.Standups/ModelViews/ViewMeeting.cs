using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatFirst.Hack.Standups.ModelViews
{
    public class ViewMeeting
    {
        public long Id { get; set; }
        /// <summary>
        /// Дата начала митинга
        /// </summary>
        public DateTime? DateStart { get; set; }
        /// <summary>
        /// Дата окончания митинга
        /// </summary>
        public DateTime? DateEnd { get; set; }

        public long? RoomId { get; set; }

        public ICollection<ViewAnswer> Answers { get; set; }

        public ViewMeeting()
        {
            this.Answers = new List<ViewAnswer>();
        }
    }
}