using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceCore.Models
{
    public class RegistrationRequest
    {
        public string EmailAddress = string.Empty;
        public string Credentials = string.Empty;
        public string Callsign = string.Empty;


        public bool Valid()
        {
            if (string.IsNullOrEmpty(EmailAddress) || string.IsNullOrEmpty(Credentials) || string.IsNullOrEmpty(Callsign))
                return false;


            if (!EmailAddress.Contains("@") || !EmailAddress.Contains("."))
                return false;

            if (Callsign.Trim().Length < 4)
                return false;

            if (Credentials.Length < 7)
                return false;

            return true;
        }
    }
}
