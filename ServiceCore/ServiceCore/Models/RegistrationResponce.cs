using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceCore.Models
{
    public class RegistrationResponce
    {
        public bool OK = false;
        public string Result = string.Empty;
        public string Token = string.Empty;
        public int CallsignID = -1;
    }
}
