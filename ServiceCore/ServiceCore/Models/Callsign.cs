using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceCore.Models
{
    public class Callsign
    {
        [Key]
        public int callsign_id { get; set; }
        public User user { get; set; }
        public string name { get; set; }
        public int enabled { get; set; }
        public DateTime created { get; set; }
    }
}
