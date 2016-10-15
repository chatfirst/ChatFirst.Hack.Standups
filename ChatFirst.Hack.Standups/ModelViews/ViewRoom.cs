using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatFirst.Hack.Standups.ModelViews
{
    public class ViewRoom
    {
        public long Id { get; set; }

        /// <summary>
        /// Идентификатор комнаты в CS
        /// </summary>
        public string RoomId { get; set; }

        /// <summary>
        /// Идентификатор команды в CS
        /// </summary>
        public string TeamId { get; set; }

        /// <summary>
        /// Планировщик HangFire
        /// </summary>
        public string Cron { get; set; }

        public ICollection<ViewMeeting> Meetings { get; set; }

        public ViewRoom()
        {
            this.Meetings = new List<ViewMeeting>();
        }
    }
}