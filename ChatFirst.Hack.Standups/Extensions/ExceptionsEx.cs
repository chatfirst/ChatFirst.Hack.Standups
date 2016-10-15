using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatFirst.Hack.Standups.Extensions
{
    public static class ExceptionsEx
    {
        public static Exception GetInnerBottomException(this Exception ex)
        {
            return ex.InnerException?.GetInnerBottomException() ?? ex;
        }
    }
}