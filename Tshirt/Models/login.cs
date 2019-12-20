using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tshirt.Models
{
    public class Login
    {
        public int loginId { get; set; }
        public string loginName { get; set; }
        public string loginRole { get; set; }
        public string loginPassword { get; set; }
        public string loginEmail { get; set; }

    }
}