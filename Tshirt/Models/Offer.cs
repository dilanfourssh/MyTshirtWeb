using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tshirt.Models
{
    public class Offer
    {
        public int offerId { get; set; }
        public string offerName { get; set; }
        public DateTime offerEntryDate { get; set; }
        public int offerNumberOfDay { get; set; }
        public string offerImage { get; set; }
        public string offerConfirmWeb { get; set; }
        public string offerDescriptions { get; set; }
        public int offerDelete { get; set; }
        public int offerAmount { get; set; }
        public string offerDeleveryAddress { get; set; }
        public string buyerConfirmOffer { get; set; }
        public int offerManId { get; set; }
    }
}