using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tshirt.Models
{
    public class RequestOrder
    {
        public int requestOrderID { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string designdate { get; set; }
        public string deleverydate { get; set; }
        public string deleveryaddress { get; set; }
        public string printcatogory { get; set; }
        public string printcolor { get; set; }
        public string discription { get; set; }
        public string designprice { get; set; }
        public string printprice { get; set; }
        public string uploadsample { get; set; }
        public int adminconfirm { get; set; }
    }
}
