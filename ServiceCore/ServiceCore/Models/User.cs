using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceCore.Models
{
    public class User
    {
        [Key]
        public int user_id { get; set; }
        public string email { get; set; }
        public string hash { get; set; }

        public int enabled { get; set; }

        public int verified { get; set; }

        public string verification_token { get; set; }

        public DateTime created { get; set; }
        public DateTime last_used { get; set; }


        public bool IsAnon()
        {
            return enabled != 0 && verified == 0 && verification_token == "ANON" && !email.Contains("@"); 
        }
    }
}
