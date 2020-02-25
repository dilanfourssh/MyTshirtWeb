using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tshirt.Models
{
    public class addproject
    {
        public int id { get; set; }
        public string category { get; set; }
        public string imgname1 { get; set; }
        public string imgname2 { get; set; }
        public string imgname3 { get; set; }
        public string zipfilename { get; set; }
        public string folderpath { get; set; }
        public int sessionid { get; set; }
        public string tittle { get; set; }
        public string smallDescription { get; set; }
        public string largeDescription { get; set; }
    }
}