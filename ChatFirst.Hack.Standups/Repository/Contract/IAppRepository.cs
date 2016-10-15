using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatFirst.Hack.Standups.Repository.Contract
{
    public interface IAppRepository : IAnswerRepository, IMeetingRepository, IRoomRepository
    {
    }
}
