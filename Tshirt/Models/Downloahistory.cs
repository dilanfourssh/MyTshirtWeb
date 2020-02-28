using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tshirt.Models
{
    public class Downloahistory
    {
        public int id { get; set; }
        public int addProjectId { get; set; }
        public string  downloademail { get; set; }
        public DateTime downloadTime { get; set; }
    }
}