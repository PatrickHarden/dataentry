using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace dataentry.Data.DBContext.Model
{
    public class Listing
    {
        public int ID { get; set; }
        public Guid RegionID { get; set; }
        public string ExternalID { get; set; }
        public string PreviewID { get;  set; }
        public string MIQID { get; set; }
        public bool IsParent { get; set; }
        public int? ParentListingID { get; set; }
        public string Name { get; set; }
        public string UsageType { get; set; }
        public string Status { get; set; }
        public int? AddressID { get; set; }        
        public DateTime? AvailableFrom { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? AssignmentFlag { get; private set; }
        
        public List<ListingData> ListingData { get; set; }
        public List<ListingBroker> ListingBroker { get; set; }
        public List<ListingImage> ListingImage { get; set; }

        [ForeignKey("RegionID")]
        public Region Region { get; set; }

        [ForeignKey("AddressID")]
        public Address Address { get; set; }

        [ForeignKey("ParentListingID")]
        public List<Listing> Spaces { get; set; }
    }
}
