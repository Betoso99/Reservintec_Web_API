using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;


namespace WebApiReserva.Utilities
{
    public class CryptoPass
    {
        public static string Hash(string value)
        {
            return BitConverter.ToString(
                System.Security.Cryptography.SHA256.Create()
                .ComputeHash(Encoding.UTF8.GetBytes(value))).Replace("-", string.Empty);
        }
    }
}