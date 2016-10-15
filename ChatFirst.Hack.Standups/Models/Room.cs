using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChatFirst.Hack.Standups.Models
{
    public class Room
    {
        [Key]
        public long Id { get; set; }

        public string BotName { get; set; }

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

        public ICollection<Meeting> Meetings { get; set; }

        public Room()
        {
            this.Meetings = new List<Meeting>();
        }
    }
}