using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tshirt.Models
{
    public class ColingOff
    {
        [key]
        public int Id { get; set; }
        public string positionVal { get; set; }
        public int positionDetailId { get; set; }
        public int coolDownCount { get; set; }
        public DateTime coolDownDate { get; set; }
    }
}