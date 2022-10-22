using dataentry.AutoGraph.Attributes;
using GraphQL.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.ViewModels.GraphQL
{
    [AutoObjectGraphType("Listing")]
    [AutoInputObjectGraphType("ListingInput")]
    [Description("Represents a listing in CBRE Global Listings")]
    public class ListingViewModel
    {
        [Description("The internal ID for the listing in the Data Entry application", TargetGraphType.ObjectGraphType)]
        [Description("The internal ID for the listing in the Data Entry application. This should match the ID of the listing you are attempting to edit, or leave as 0 if this is a new listing", TargetGraphType.InputObjectGraphType)]
        public int Id { get; set; }

        [Description("The external ID for the listing when publishing to the Store API")]
        public string ExternalId { get; set; }

        [Description("The external ID for the listing when creating a preview listing in the Store API")]
        public string PreviewId { get; set; }

        [Description("The ID of this property in MarketIQ.")]
        public string MiqId { get; set; }

        [Ignore(TargetGraphType.InputObjectGraphType)]
        [FieldType(typeof(CustomDateTimeGraphType))]
        [Description("The date this listing was first created")]
        public DateTime DateCreated { get; set; }

        [Ignore(TargetGraphType.InputObjectGraphType)]
        [FieldType(typeof(CustomDateTimeGraphType))]
        [Description("The date this listing was last updated")]
        public DateTime DateUpdated { get; set; }

        [Ignore(TargetGraphType.InputObjectGraphType)]
        [Description("The list of users with permission to view and edit this listing")]
        public IEnumerable<UserViewModel> Users { get; set; }

        [Ignore(TargetGraphType.ObjectGraphType)]
        [Description("The new list of user emails to allow to view and edit this listing")]
        public IEnumerable<string> UserNames { get; set; }

        [Ignore(TargetGraphType.InputObjectGraphType)]
        [Description("The list of teams with permission to view and edit this listing")]
        public IEnumerable<TeamViewModel> Teams { get; set; }

        [Ignore(TargetGraphType.ObjectGraphType)]
        [Description("The new list of team names to allow to view and edit this listing")]
        public IEnumerable<string> TeamNames { get; set; }

        [Ignore(TargetGraphType.InputObjectGraphType)]
        [Description("The email address of the original creator of this listing")]
        public string Owner { get; set; }

        public string PropertyRecordName { get; set; }
        public string PropertyName { get; set; }

        public string ConfigId { get; set; }

        [Description("The listing status indicating whether this property is available on the market. Current accepted values are Available and Unavailable.")]
        public string Status { get; set; }

        [FieldType(typeof(CustomDateTimeGraphType))]
        [Description("The date this listing becomes avaialable for purchase or lease.")]
        public DateTime? AvailableFrom { get; set; }

        public string Street { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string StateOrProvince { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string EnergyRating { get; set; }
        public IEnumerable<ExternalRatingsViewModel> ExternalRatings { get; set;}
        public IEnumerable<TextTypeViewModel> BuildingDescription { get; set; }
        public IEnumerable<TextTypeViewModel> LocationDescription { get; set; }
        public bool Published { get; set; }
        public string PropertyType { get; set; }
        public string PropertySubType { get; set; }
        public string PropertyUseClass { get; set; }
        public string ListingType { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
        public bool? SyndicationFlag { get; set; }
        public string SyndicationMarket { get; set; }
        public string Website { get; set; }
        public string Video { get; set; }
        public string WalkThrough { get; set; }
        public string ImportedData { get; set; }
        public string Operator { get; set; }
        public int? Floors { get; set; }
        public int? YearBuilt { get; set; }
        public IEnumerable<TextTypeViewModel> Headline { get; set; }
        public bool? IsBulkUpload { get; set; }
        public bool? IsDeleted { get; set; }
        public string BulkUploadFileName { get; set; }
        public string State { get; set; }
        public DateTimeOffset DatePublished { get; set; }
        public DateTimeOffset DateListed { get; set; }
        public string PreviewState { get; set; }
        public IEnumerable<MediaViewModel> Brochures { get; set; }
        public IEnumerable<ImagesViewModel> Photos { get; set; }
        public IEnumerable<ImagesViewModel> Floorplans { get; set; }
        public IEnumerable<MediaViewModel> EpcGraphs { get; set; }
        public IEnumerable<HighlightViewModel> Highlights { get; set; }
        public SpecificationsViewModel Specifications { get; set; }
        public IEnumerable<PropertySizesViewModel> PropertySizes { get; set; }
        public IEnumerable<SpacesViewModel> Spaces { get; set; }
        public IEnumerable<MicroMarketsViewModel> MicroMarkets { get; set; }
        public IEnumerable<ChargesAndModifiersViewModel> ChargesAndModifiers { get; set; }
        public IEnumerable<ContactsViewModel> Contacts { get; set; }
        public DataSourceViewModel DataSource { get; set; }
        public int SpacesCount { get; set; }
        public string ExternalPublishUrl { get; set; }
        public string PreviewSearchApiEndPoint { get; set; }
        public string ExternalPreviewUrl { get; set; }
        public ParkingsViewModel Parkings { get; set; }
        public IEnumerable<string> Aspects { get; set; }
        public IEnumerable<PointsOfInterestsViewModel> PointsOfInterests { get; set; }
        public IEnumerable<TransportationTypesViewModel> TransportationTypes { get; set; }
        public string RegionID { get; set; }  
        public ListingAssignmentViewModel ListingAssignment { get; set; }
        
        [JsonIgnore]
        public IEnumerable<ListingDeltaViewModel> Deltas { get; set; }
        public IEnumerable<AlternatePostalAddressViewModel> AlternatePostalAddresses { get; set; }
    }
}
