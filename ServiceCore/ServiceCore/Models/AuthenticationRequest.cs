using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceCore.Models
{
    public class AuthenticationRequest
    {
        public string EmailAddress = string.Empty;
        public string Credentials = string.Empty;
        public int CallsignIndex = 0;
    }
}
