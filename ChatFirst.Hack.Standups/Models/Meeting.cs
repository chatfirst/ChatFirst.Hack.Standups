using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatFirst.Hack.Standups.Models
{
    /// <summary>
    /// Фактически начатый/завершенный митинг
    /// </summary>
    public class Meeting
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
        public Room Room { get; set; }

        public ICollection<Answer> Answers { get; set; }

        public Meeting()
        {
            this.Answers = new List<Answer>();
        }
    }
}