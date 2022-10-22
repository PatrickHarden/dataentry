using ClosedXML.Attributes;
using dataentry.Extensions;

namespace dataentry.ViewModels
{
    public class DEReport
    {
        #region private
        private string listingOwners;
        private string energyRating;
        private string availabilityFloorNames;
        private string availabilityFloors;
        private string availabilityFloorPrice;
        private string brokerName;
        private string brokerOffice;
        private string brokerLicenseNumber;
        private string brokerEmail;
        private string brokerPhone;
        #endregion 
        [XLColumn(Header = "ID")]
        public int? Id { get; set; }
        [XLColumn(Header = "Data Entry Listing URL")]
        public string ListingURL { get; set; }
        [XLColumn(Header = "Listings Owners")]
        public string ListingOwners { 
            get { return listingOwners?.ReportCellRowFormat(); } 
            set 
            {
                listingOwners = value; }
        }
        [XLColumn(Header = "Property Type")]
        public string PropertyType { get; set; }
        [XLColumn(Header = "Property Subtype")]
        public string PropertySubType { get; set; }
        [XLColumn(Header = "Listing Type")]
        public string ListingType { get; set; }
        [XLColumn(Header = "Publishing Status")]
        public string PublishingStatus { get; set; }
        [XLColumn(Header = "Market")]
        public string Market { get; set; }
        [XLColumn(Header = "Property Record Name")]
        public string PropertyRecordName { get; set; }
        [XLColumn(Header = "Building Display Name")]
        public string BuildingDisplayName { get; set; }
        [XLColumn(Header = "Street 1")]
        public string Street1 { get; set; }
        [XLColumn(Header = "Street 2")]
        public string Street2 { get; set; }
        [XLColumn(Header = "City")]
        public string City { get; set; }
        [XLColumn(Header = "StateProvince")]
        public string StateProvince { get; set; }
        [XLColumn(Header = "PostalCode")]
        public string PostalCode { get; set; }
        [XLColumn(Header = "Longitude")]
        public decimal? Longitude { get; set; }
        [XLColumn(Header = "Latitude")]
        public decimal? Latitude { get; set; }
        [XLColumn(Header = "Primary Photo")]
        public string PrimaryPhoto { get; set; }
        [XLColumn(Header = "Floorplan")]
        public string FloorPlan { get; set; }
        [XLColumn(Header = "Brochure")]
        public string Brochure { get; set; }
        [XLColumn(Header = "EPC Pdf")]
        public string EPCPdf { get; set; }
        [XLColumn(Header = "Energy Rating")]
        public string EnergyRating { get { return energyRating?.ReportCellRowFormat(); } set { energyRating = value; } }
        [XLColumn(Header = "Minimum Space")]
        public string MinimumSpace { get; set; }
        [XLColumn(Header = "Total Space Available")]
        public string TotalSpaceAvailable { get; set; }
        [XLColumn(Header = "Minimum Lease Price")]
        public string MinimumLeasePrice { get; set; }
        [XLColumn(Header = "Maximum Lease Price")]
        public string MaximumLeasePrice { get; set; }
        [XLColumn(Header = "Sale Price")]
        public string SalePrice { get; set; }
        [XLColumn(Header = "Contact Broker for Pricing")]
        public string ContactBrokerForPricing { get; set; }
        [XLColumn(Header = "Availability/Floor Names")]
        public string AvailabilityFloorNames { get { return availabilityFloorNames?.ReportCellRowFormat(); } set { availabilityFloorNames = value.ReportCellRowFormat(); } }
        [XLColumn(Header = "Availability/Floor Size")]
        public string AvailabilityFloors { get { return availabilityFloors?.ReportCellRowFormat(); } set { availabilityFloors = value.ReportCellRowFormat(); } }
        [XLColumn(Header = "Availability/Floor Price")]
        public string AvailabilityFloorPrice { get { return availabilityFloorPrice?.ReportCellRowFormat(); } set { availabilityFloorPrice = value.ReportCellRowFormat(); } }
        [XLColumn(Header = "Broker Name")]
        public string BrokerName { get { return brokerName?.ReportCellRowFormat(); } set { brokerName = value.ReportCellRowFormat(); } }
        [XLColumn(Header = "Broker Office")]
        public string BrokerOffice { get { return brokerOffice?.ReportCellRowFormat(); } set { brokerOffice = value.ReportCellRowFormat(); } }
        [XLColumn(Header = "Broker License Number")]
        public string BrokerLicenseNumber { get { return brokerLicenseNumber?.ReportCellRowFormat(); } set { brokerLicenseNumber = value.ReportCellRowFormat(); } }
        [XLColumn(Header = "Broker Email")]
        public string BrokerEmail { get { return brokerEmail?.ReportCellRowFormat(); } set { brokerEmail = value.ReportCellRowFormat(); } }
        [XLColumn(Header = "Broker Phone")]
        public string BrokerPhone { get { return brokerPhone?.ReportCellRowFormat(); } set { brokerPhone = value.ReportCellRowFormat(); } }
        [XLColumn(Header = "Created Date")]
        public string CreatedDate { get; set; }
        [XLColumn(Header = "Last Updated Date")]
        public string LastUpdatedDate { get; set; }
        [XLColumn(Header = "Last Published Date")]
        public string LastPublishedDate { get; set; }
        [XLColumn(Header = "Time Since Last Publish (Days)")]
        public string TimeSinceLastPublish { get; set; }
    }
}
