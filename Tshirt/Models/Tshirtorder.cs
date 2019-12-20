using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tshirt.Models
{
    
    public class Tshirtorder
    {
        public int tshirtorderId { get; set; }
        public string orderDescription { get; set; }
        public string customerName { get; set; }
        public string email { get; set; }
        public double width { get; set; }
        public double hight { get; set; }
        public string address { get; set; }
        public string phoneNumber { get; set; }
    }
}