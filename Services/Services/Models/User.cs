using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Services.Models
{
    public class User
    {
        public int id = -1;
        public string email = string.Empty;
        public string hash = string.Empty;
        public bool enabled = false;
    }
}