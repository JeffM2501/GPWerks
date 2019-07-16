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
    }
}