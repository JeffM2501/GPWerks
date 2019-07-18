using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceCore.Models
{
    public class AnonUser
    {
        [Key]
        public int anon_user_id { get; set; }

        public string name { get; set; }

        public string token { get; set; }

        public DateTime created { get; set; }

        public DateTime last_used { get; set; }

        public int enabled { get; set; }
    }
}
