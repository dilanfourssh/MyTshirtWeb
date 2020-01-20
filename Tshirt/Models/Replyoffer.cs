using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tshirt.Models
{
    public class Replyoffer
    {
        public int replyOfferId { get; set; }
        public string offerdescription { get; set; }
        public string enterspecialNotice { get; set; }
        public int enterDurations { get; set; }
        public int enterAmount { get; set; }
        public int offerId { get; set; }
        public int sessionId { get; set; }
        public string sampleImage { get; set; }
    }
}