using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiReserva.Utilities
{
    public static class LogUtilities
    {
        public static IEnumerable<object> MergeLogResult(Logger log, object table)
        {
            List<object> ls = new List<object>() { log, table };
            return ls.ToList();
        }

        public static void Good(Logger log)
        {
            log.Ok = true;
            log.ErrorMessage = "";
        }

    }
}