//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Tshirt.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Offer
    {
        public int offerId { get; set; }
        public string offerName { get; set; }
        public Nullable<System.DateTime> offerEntryDate { get; set; }
        public Nullable<int> offerNumberOfDay { get; set; }
        public string offerImage { get; set; }
        public string offerConfirmWeb { get; set; }
        public string offerDescriptions { get; set; }
        public Nullable<int> offerDelete { get; set; }
    }
}
