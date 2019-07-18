using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace ServiceCore.Models
{
    public class AccessToken
    {
        [Key]
        public int access_token_id { get; set; }

        public DateTime expires { get; set; }

        public string token { get; set; }

        public int callsign_index { get; set; }

        public User user { get; set; }
    }
}
