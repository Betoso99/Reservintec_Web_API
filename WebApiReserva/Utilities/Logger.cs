using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiReserva.Utilities
{
    public class Logger
    {
        public Logger(bool ok, string message)
        {
            Ok = ok;
            ErrorMessage = message;
        }

        public Logger()
        {
            Ok = true;
            ErrorMessage = "";
        }

        public bool Ok { get; set; }
        public string ErrorMessage { get; set; }
    }
}