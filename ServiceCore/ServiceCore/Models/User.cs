using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceCore.Models
{
    public class User
    {
        public int id { get; set; }
        public string email { get; set; }
        public string hash { get; set; }

        public int enabled { get; set; }

    }
}
