using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Services.Models
{
    public class RegistrationRequest
    {
        public string EmailAddress = string.Empty;
        public string Credentials = string.Empty;
        public string Callsign = string.Empty;

        public bool Valid()
        {
            if (String.IsNullOrEmpty(Credentials) || String.IsNullOrEmpty(EmailAddress) || String.IsNullOrEmpty(Callsign))
                return false;

        //    if (Credentials.Length < 7 || Credentials.First(x => char.IsDigit(x)) != null) )

            return true;
        }
    }
}