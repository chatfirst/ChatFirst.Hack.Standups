using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ChatFirst.Hack.Standups.Services
{
    using Models;

    public interface IMeetingService
    {
        Task StartMeetingAsync(long roomId);

        Task QuitMeetingAsync(string sparkRoomId);

        Task<IEnumerable<Meeting>> GetMeetingsByRoomId(long roomId);
    }
}