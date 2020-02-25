using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tshirt.Models
{
    public class Register
    {
        public int Id { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string password2 { get; set; }
        public int logtype { get; set; }
    }
}