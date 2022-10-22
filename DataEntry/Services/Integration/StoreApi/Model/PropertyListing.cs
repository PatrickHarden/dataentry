using dataentry.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace dataentry.Services.Integration.StoreApi.Model
{
    public partial class PropertyListingEnvelope
    {
        [JsonProperty("PropertyListing")]
        public PropertyListing PropertyListing { get; set; }
    }

    public partial class PropertyListing
    {
        [JsonProperty("Common.ActualAddress")]
        public CommonActualAddress CommonActualAddress { get; set; }

        [JsonProperty("Common.Agency")]
        public CommonAgency CommonAgency { get; set; }

        [JsonProperty("Common.Agents")]
        public List<CommonAgentElement> CommonAgents { get; set; }

        [JsonProperty("Common.ContactGroup")]
        public CommonContactGroup CommonContactGroup { get; set; }

        [JsonProperty("Common.Aspects")]
        public List<string> CommonAspects { get; set; }

        [JsonProperty("Common.AvailableFrom")]
        public string CommonAvailableFrom { get; set; }

        [JsonProperty("Common.Availability")]
        public CommonAvailability CommonAvailability { get; set; }

        [JsonProperty("Common.Brochures")]
        public List<CommonBrochureElement> CommonBrochures { get; set; }

        [JsonProperty("Common.Charges")]
        public List<CommonCharge> CommonCharges { get; set; }

        [JsonProperty("Common.Coordinate")]
        public CommonCoordinate CommonCoordinate { get; set; }

        [JsonProperty("Common.GeoLocation")]
        public CommonGeoLocation CommonGeoLocation { get; set; }

        [JsonProperty("Common.Created")]
        public string CommonCreated { get; set; }

        [JsonProperty("Common.Sizes")]
        public List<CommonSize> CommonSizes { get; set; }

        [JsonProperty("Common.FloorPlans")]
        public List<CommonImageType> CommonFloorPlans { get; set; }

        [JsonProperty("Common.FloorsAndUnits")]
        public List<CommonFloorsAndUnit> CommonFloorsAndUnits { get; set; }
        
        [JsonProperty("Common.Micromarket")]
        public CommonMicromarket CommonMicromarket { get; set; }

        [JsonProperty("Common.Highlights")]
        public List<CommonHighlight> CommonHighlights { get; set; }

        [JsonProperty("Common.HomeSite")]
        public string CommonHomeSite { get; set; }

        [JsonProperty("Common.LandSize")]
        public List<CommonLandSizeElement> CommonLandSize { get; set; }

        [JsonProperty("Common.LastProcessed")]
        public string CommonLastProcessed { get; set; }

        [JsonProperty("Common.LastUpdated")]
        public string CommonLastUpdated { get; set; }

        [JsonProperty("Common.SourceLastupdated")]
        public string CommonSourceLastupdated { get; set; }

        [JsonProperty("Common.LeaseTypes")]
        public List<string> CommonLeaseTypes { get; set; }

        [JsonProperty("Common.LeaseRateType")]
        public string CommonLeaseRateType { get; set; }

        [JsonProperty("Common.LongDescription")]
        public List<CommonCommentElement> CommonLongDescription { get; set; }

        [JsonProperty("Common.MaximumSize")]
        public List<CommonLandSizeElement> CommonMaximumSize { get; set; }

        [JsonProperty("Common.MinimumSize")]
        public List<CommonLandSizeElement> CommonMinimumSize { get; set; }

        [JsonProperty("Common.ListingCount")]
        public long CommonListingCount { get; set; }

        [JsonProperty("Common.NumberOfStoreys")]
        public int? CommonNumberOfStoreys { get; set; }

        [JsonProperty("Common.YearBuilt")]
        public int? CommonYearBuilt { get; set; }

        [JsonProperty("Common.IsParent")]
        public bool CommonIsParent { get; set; }

        [JsonProperty("Common.NumberOfBedrooms")]
        public int CommonNumberOfBedrooms { get; set; }

        [JsonProperty("Common.Photos")]
        public List<CommonImageType> CommonPhotos { get; set; }

        [JsonProperty("Common.PrimaryKey")]
        public string CommonPrimaryKey { get; set; }

        [JsonProperty("Common.PropertyTypes")]
        public List<string> CommonPropertyTypes { get; set; }

        [JsonProperty("Common.RestrictedSites")]
        public List<string> CommonRestrictedSites { get; set; }

        [JsonProperty("Common.Strapline")]
        public IEnumerable<CommonCommentElement> CommonStrapline { get; set; }

        [JsonProperty("Common.TotalSize")]
        public List<CommonLandSizeElement> CommonTotalSize { get; set; }

        [JsonProperty("Common.EnergyPerformanceInformation")]
        public List<CommonBrochureElement> CommonEnergyPerformanceInformation { get; set; }

        [JsonProperty("Common.EnergyPerformanceData")]
        public CommonEnergyPerformanceData CommonEnergyPerformanceData { get; set; }

        [JsonProperty("Common.UsageType")]
        public string CommonUsageType { get; set; }

        [JsonProperty("Common.Website")]
        public string CommonWebsite { get; set; }

        [JsonProperty("Common.Walkthrough")]
        public string CommonWalkthrough { get; set; }

        [JsonProperty("Common.ParentProperty")]
        public string CommonParentProperty { get; set; }

        [JsonProperty("UnitedKingdom.VATPayable")]
        public bool UnitedKingdomVatPayable { get; set; }

        [JsonProperty("UnitedKingdom.UseClass")]
        public List<string> UnitedKingdomUseClass { get; set; }

        [JsonProperty("Canada.UseClass")]
        public List<string> CanadaUseClass { get; set; }

        [JsonProperty("Common.LeaseInfo")]
        public CommonLeaseInfo CommonLeaseInfo { get; set; }

        [JsonProperty("UnitedKingdom.BusinessRatesInfo")]
        public UnitedKingdomBusinessRatesInfo UnitedKingdomBusinessRatesInfo { get; set; }

        [JsonProperty("Industrial.LoadingDocks")]
        public long IndustrialLoadingDocks { get; set; }

        [JsonProperty("Industrial.LoadingDocksUnit")]
        public string IndustrialLoadingDocksUnit { get; set; }

        [JsonProperty("Industrial.LoadingDoors")]
        public long IndustrialLoadingDoors { get; set; }

        [JsonProperty("Common.LocationDescription")]
        public List<CommonCommentElement> CommonLocationDescription { get; set; }

        [JsonProperty("Common.PointsOfInterest")]
        public List<CommonPointsOfInterest> CommonPointsOfInterest { get; set; }

        [JsonProperty("Common.PointsOfInterests")]
        public List<CommonPointsOfInterests> CommonPointsOfInterests { get; set; }
        [JsonProperty("Common.TransportationTypes")]
        public List<CommonTransportationTypes> CommonTransportationTypes { get; set; }

        [JsonProperty("Common.InternalParkingSpaces")]
        public long CommonInternalParkingSpaces { get; set; }

        [JsonProperty("Common.ExternalParkingSpaces")]
        public long CommonExternalParkingSpaces { get; set; }

        [JsonProperty("Common.PropertySubType")]
        public string CommonPropertySubType { get; set; }

        [JsonProperty("Common.Status")]
        public string CommonStatus { get; set; }

        [JsonProperty("Common.Parking")]
        public CommonParking CommonParking { get; set; }

        [JsonProperty("Common.Tenancy")]
        public string CommonTenancy { get; set; }

        [JsonProperty("Common.Comments")]
        public List<CommonCommentElement> CommonComments { get; set; }

        [JsonProperty("Common.ListingOrder")]
        public long CommonListingOrder { get; set; }

        [JsonProperty("Common.NumberOfLots")]
        public long CommonNumberOfLots { get; set; }

        [JsonProperty("Common.VideoLinks")]
        public List<CommonVideoLink> CommonVideoLinks { get; set; }

        [JsonProperty("Common.BuildingOperator")]
        public string CommonBuildingOperator { get; set; }

        [JsonProperty("Common.BuildingOperatorID")]
        public string CommonBuildingOperatorId { get; set; }

        [JsonProperty("Common.RelatedListingOffice")]
        public string CommonRelatedListingOffice { get; set; }

        [JsonProperty("Common.Portfolios")]
        public List<string> CommonPortfolios { get; set; }

        [JsonProperty("Common.PublishExternally")]
        public bool CommonPublishExternallyExternal { get; set; }
        
        [JsonProperty("Common.SourceSystem")]
        public CommonSourceSystem CommonSourceSystem { get; set; }

        // deleteBody fields
        [JsonProperty("Common.Source")]
        public string CommonSource { get; set; }

        [JsonProperty("Common.EntityType")]
        public string CommonEntityType { get; set; }
    }

    public partial class DeleteBody
    {
        public PropertyListing PropertyListing { get; set; }
    }


    public partial class CommonActualAddress
    {
        [JsonProperty("Common.Line1")]
        public string CommonLine1 { get; set; }

        [JsonProperty("Common.Line2")]
        public string CommonLine2 { get; set; }

        [JsonProperty("Common.Line3")]
        public string CommonLine3 { get; set; }

        [JsonProperty("Common.Line4")]
        public string CommonLine4 { get; set; }

        [JsonProperty("Common.Locallity")]
        public string CommonLocallity { get; set; }

        [JsonProperty("Common.Region")]
        public string CommonRegion { get; set; }

        [JsonProperty("Common.Zoning")]
        public string CommonZoning { get; set; }

        [JsonProperty("Common.PostalAddresses")]
        public List<CommonPostalAddress> CommonPostalAddresses { get; set; }

        [JsonProperty("Common.Country")]
        public string CommonCountry { get; set; }

        [JsonProperty("Common.PostCode")]
        public string CommonPostCode { get; set; }
    }

    public partial class CommonPostalAddress
    {
        [JsonProperty("Common.Language")]
        public string CommonLanguage { get; set; }

        [JsonProperty("Common.Line1")]
        public string CommonLine1 { get; set; }

        [JsonProperty("Common.Line2")]
        public string CommonLine2 { get; set; }

        [JsonProperty("Common.Line3")]
        public string CommonLine3 { get; set; }

        [JsonProperty("Common.Line4")]
        public string CommonLine4 { get; set; }

        [JsonProperty("Common.Locallity")]
        public string CommonLocallity { get; set; }

        [JsonProperty("Common.Region")]
        public string CommonRegion { get; set; }

        [JsonProperty("Common.Zoning")]
        public string CommonZoning { get; set; }
    }

    public partial class CommonAgency
    {
        [JsonProperty("Common.AgentAddress")]
        public CommonAgentAddress CommonAgentAddress { get; set; }

        [JsonProperty("Common.AgencyName")]
        public string CommonAgencyName { get; set; }

        [JsonProperty("Common.EmailAddress")]
        public string CommonEmailAddress { get; set; }

        [JsonProperty("Common.TelephoneNumber")]
        public string CommonTelephoneNumber { get; set; }
    }

    public partial class CommonAgentAddress
    {
        [JsonProperty("Common.Line1")]
        public string CommonLine1 { get; set; }

        [JsonProperty("Common.Line2")]
        public string CommonLine2 { get; set; }

        [JsonProperty("Common.Line3")]
        public string CommonLine3 { get; set; }

        [JsonProperty("Common.Line4")]
        public string CommonLine4 { get; set; }

        [JsonProperty("Common.Locallity")]
        public string CommonLocallity { get; set; }

        [JsonProperty("Common.Country")]
        public string CommonCountry { get; set; }

        [JsonProperty("Common.PostCode")]
        public string CommonPostCode { get; set; }
    }

    public partial class CommonAgentElement
    {
        [JsonProperty("Common.EmailAddress")]
        public string CommonEmailAddress { get; set; }

        [JsonProperty("Common.AgentName")]
        public string CommonAgentName { get; set; }

        [JsonProperty("Common.TelephoneNumber")]
        public string CommonTelephoneNumber { get; set; }

        [JsonProperty("Common.Website", NullValueHandling = NullValueHandling.Ignore)]
        public string CommonWebsite { get; set; }

        [JsonProperty("Common.Avatar", NullValueHandling = NullValueHandling.Ignore)]
        public string CommonAvatar { get; set; }

        [JsonProperty("Common.AgentTitle", NullValueHandling = NullValueHandling.Ignore)]
        public List<CommonCommentElement> CommonAgentTitle { get; set; }

        [JsonProperty("Common.LicenseNumber")]
        public string CommonLicenseNumber { get; set; }

        [JsonProperty("Common.AgentOffice")]
        public string CommonAgentOffice { get; set; }
    }

    public partial class CommonCommentElement
    {
        [JsonProperty("Common.CultureCode")]
        public string CommonCultureCode { get; set; }

        [JsonProperty("Common.Text")]
        public string CommonText { get; set; }
    }

    public partial class CommonAvailability
    {
        [JsonProperty("Common.AvailabilityKind")]
        public string CommonAvailabilityKind { get; set; }

        [JsonProperty("Common.AvailableOnDescription")]
        public string CommonAvailableOnDescription { get; set; }

        [JsonProperty("Common.AvailabilityDate")]
        public string CommonAvailabilityDate { get; set; }

        [JsonProperty("Common.MonthsAfterLeaseOrSale")]
        public long CommonMonthsAfterLeaseOrSale { get; set; }
    }

    public partial class CommonBrochureElement
    {
        [JsonProperty("Common.Uri")]
        public string CommonUri { get; set; }

        [JsonProperty("Common.CultureCode")]
        public string CommonCultureCode { get; set; }

        [JsonProperty("Common.UriExternal")]
        public bool CommonUriExternal { get; set; }

        [JsonProperty("Common.BrochureName")]
        public string CommonBrochureName { get; set; }
    }

    public partial class CommonCharge
    {
        [JsonProperty("Common.ChargeKind")]
        public string CommonChargeKind { get; set; }

        [JsonProperty("Common.CurrencyCode")]
        public string CommonCurrencyCode { get; set; }

        [JsonProperty("Common.Interval")]
        public string CommonInterval { get; set; }

        [JsonProperty("Common.Amount")]
        public double CommonAmount { get; set; }

        [JsonProperty("Common.AmountKind")]
        public string CommonAmountKind { get; set; }

        [JsonProperty("Common.Year")]
        [JsonConverter(typeof(IntOrStringJsonConverter))]
        public int? CommonYear { get; set; }

        [JsonProperty("Common.PerUnit")]
        public string CommonPerUnit { get; set; }

        [JsonProperty("Common.TaxModifer")]
        public string CommonTaxModifer { get; set; }

        [JsonProperty("Common.PaidBy")]
        public string CommonPaidBy { get; set; }

        [JsonProperty("Common.DependentCharge")]
        public string CommonDependentCharge { get; set; }

        [JsonProperty("Common.ChargeModifer")]
        public string CommonChargeModifer { get; set; }

        [JsonProperty("Common.OnApplication")]
        public bool CommonOnApplication { get; set; }

        [JsonProperty("Common.Exact")]
        public bool CommonExact { get; set; }
    }

    public partial class CommonContactGroup
    {
        [JsonProperty("Common.GroupName")]
        public List<CommonCommentElement> CommonGroupName { get; set; }

        [JsonProperty("Common.Address")]
        public CommonActualAddress CommonAddress { get; set; }

        [JsonProperty("Common.GroupType")]
        public string CommonGroupType { get; set; }

        [JsonProperty("Common.Website")]
        public string CommonWebsite { get; set; }

        [JsonProperty("Common.Avatar")]
        public string CommonAvatar { get; set; }

        [JsonProperty("Common.Contacts")]
        public List<CommonAgentElement> CommonContacts { get; set; }

        [JsonProperty("Common.ArrangeViewingContacts")]
        public List<CommonAgentElement> CommonArrangeViewingContacts { get; set; }
    }

    public partial class CommonCoordinate
    {
        [JsonProperty("lat")]
        public decimal Lat { get; set; }

        [JsonProperty("lon")]
        public decimal Lon { get; set; }
    }

    public partial class CommonEnergyPerformanceData
    {
        [JsonProperty("UnitedKingdom.EnergyPerformanceCertificateUri")]
        public Uri UnitedKingdomEnergyPerformanceCertificateUri { get; set; }

        [JsonProperty("Germany.EnergyPerformanceCertificateType")]
        public string GermanyEnergyPerformanceCertificateType { get; set; }

        [JsonProperty("Germany.EnergyPerformanceCertificateUri")]
        public Uri GermanyEnergyPerformanceCertificateUri { get; set; }

        [JsonProperty("Germany.EnergyPerformanceCertificateExpires")]
        public string GermanyEnergyPerformanceCertificateExpires { get; set; }

        [JsonProperty("Germany.ConstructionYear")]
        public double GermanyConstructionYear { get; set; }

        [JsonProperty("Germany.MajorEnergySources")]
        public List<string> GermanyMajorEnergySources { get; set; }

        [JsonProperty("Germany.TotalEnergy")]
        public List<GermanyEnergy> GermanyTotalEnergy { get; set; }

        [JsonProperty("Germany.HeatEnergy")]
        public List<GermanyEnergy> GermanyHeatEnergy { get; set; }

        [JsonProperty("Germany.ElectricalEnergy")]
        public List<GermanyEnergy> GermanyElectricalEnergy { get; set; }

        [JsonProperty("Common.CertificateType")]
        public string CommonCertificateType { get; set; }

        [JsonProperty("Common.ExternalRatings")]
        public List<CommonExternalRatings> CommonExternalRatings { get; set; }
    }

    public partial class GermanyEnergy
    {
        [JsonProperty("Common.EnergyUnits")]
        public string CommonEnergyUnits { get; set; }

        [JsonProperty("Common.Amount")]
        public double CommonAmount { get; set; }

        [JsonProperty("Common.Interval")]
        public string CommonInterval { get; set; }

        [JsonProperty("Common.PerUnit")]
        public string CommonPerUnit { get; set; }
    }

    public partial class CommonImageType
    {
        [JsonProperty("Common.ImageCaption")]
        public string CommonImageCaption { get; set; }

        [JsonProperty("Common.AddWatermark")]
        public bool CommonAddWatermark { get; set; }

        [JsonProperty("Common.ImageResources")]
        public List<CommonImageResource> CommonImageResources { get; set; }
    }

    public partial class CommonImageResource
    {
        [JsonProperty("Common.Image.Height")]
        public long CommonImageHeight { get; set; }

        [JsonProperty("Common.Resource.Uri")]
        public string CommonResourceUri { get; set; }

        [JsonProperty("Common.Image.Width")]
        public long CommonImageWidth { get; set; }

        [JsonProperty("Common.Breakpoint")]
        public string CommonBreakpoint { get; set; }

        [JsonProperty("Source.Uri")]
        public string SourceUri { get; set; }
    }

    public partial class CommonFloorsAndUnit
    {
        [JsonProperty("Common.SubdivisionName")]
        public List<CommonSubdivisionName> CommonSubdivisionName { get; set; }

        [JsonProperty("Common.Areas")]
        public List<CommonLandSizeElement> CommonAreas { get; set; }

        [JsonProperty("Common.Unit.Status")]
        public string CommonUnitStatus { get; set; }

        [JsonProperty("Common.Unit.Use")]
        public string CommonUnitUse { get; set; }

        [JsonProperty("Common.Vacancy")]
        public string CommonVacancy { get; set; }

        [JsonProperty("Common.ApproxArea")]
        public bool CommonApproxArea { get; set; }

        [JsonProperty("Common.AvailableFrom")]
        public string CommonAvailableFrom { get; set; }

        [JsonProperty("Common.Availability")]
        public CommonAvailability CommonAvailability { get; set; }

        [JsonProperty("Common.Charges")]
        public List<CommonCharge> CommonCharges { get; set; }

        [JsonProperty("Common.SpaceDescription")]
        public List<CommonCommentElement> CommonSpaceDescription { get; set; }

        [JsonProperty("Common.LeaseTypes")]
        public string CommonLeaseTypes { get; set; }

        [JsonProperty("Common.UnitBrochures")]
        public List<CommonBrochureElement> CommonBrochures { get; set; }

        [JsonProperty("Common.UnitFloorPlans")]
        public List<CommonImageType> CommonFloorPlans { get; set; }

        [JsonProperty("Common.UnitPhotos")]
        public List<CommonImageType> CommonPhotos { get; set; }

        [JsonProperty("Common.Identifier")]
        public string CommonIdentifier { get; set; }
        
        [JsonProperty("Common.UnitWalkthrough")]
        public string CommonWalkthrough { get; set; }
        
        [JsonProperty("Common.VideoLinks")]
        public List<CommonVideoLink> CommonVideoLinks { get; set; }
        
        [JsonProperty("Common.Sizes")]
        public List<CommonSize> CommonSizes { get; set; }
    }

    public partial class CommonLandSizeElement
    {
        [JsonProperty("Common.Units")]
        public string CommonUnits { get; set; }

        [JsonProperty("Common.Area")]
        public decimal CommonArea { get; set; }

        [JsonProperty("Common.MinArea")]
        public decimal? CommonMinArea { get; set; }

        [JsonProperty("Common.MaxArea")]
        public decimal? CommonMaxArea { get; set; }
    }

    public partial class CommonSubdivisionName
    {
        [JsonProperty("Common.CultureCode")]
        public string CommonCultureCode { get; set; }

        [JsonProperty("Common.Text")]
        public string CommonText { get; set; }

        [JsonProperty("Common.UnitDisplayName")]
        public string CommonUnitDisplayName { get; set; }
    }

    public partial class CommonGeoLocation
    {
        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }

        [JsonProperty("Common.Exact")]
        public bool CommonExact { get; set; }
    }

    public partial class Geometry
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("coordinates")]
        public List<decimal> Coordinates { get; set; }
    }

    public partial class CommonHighlight
    {
        [JsonProperty("Common.Highlight")]
        public List<CommonCommentElement> CommonHighlights { get; set; }
    }

    public partial class CommonMicromarket
    {
        [JsonProperty("Common.MicromarketName")]
        public string CommonMicromarketName { get; set; }

        [JsonProperty("Common.SubMarketName")]
        public string CommonSubMarketName { get; set; }
    }

    public partial class CommonLeaseInfo
    {
        [JsonProperty("Common.LeaseContractStartDate")]
        public string CommonLeaseContractStartDate { get; set; }

        [JsonProperty("Common.LeaseContractEndDate")]
        public string CommonLeaseContractEndDate { get; set; }

        [JsonProperty("Common.LeaseContractBreakDate")]
        public string CommonLeaseContractBreakDate { get; set; }

        [JsonProperty("Common.LeaseRentReviewCycle")]
        public long CommonLeaseRentReviewCycle { get; set; }

        [JsonProperty("Common.LeaseRentNextReviewDate")]
        public string CommonLeaseRentNextReviewDate { get; set; }
    }

    public partial class CommonParking
    {
        [JsonProperty("Common.Ratio")]
        public double CommonRatio { get; set; }

        [JsonProperty("Common.RatioPer")]
        public double CommonRatioPer { get; set; }

        [JsonProperty("Common.RatioPerUnit")]
        public string CommonRatioPerUnit { get; set; }

        [JsonProperty("Common.ParkingDetails")]
        public List<CommonParkingDetail> CommonParkingDetails { get; set; }
    }

    public partial class CommonParkingDetail
    {
        [JsonProperty("Common.ParkingType")]
        public string CommonParkingType { get; set; }

        [JsonProperty("Common.ParkingSpace")]
        public double CommonParkingSpace { get; set; }

        [JsonProperty("Common.ParkingCharge")]
        public List<CommonParkingCharge> CommonParkingCharge { get; set; }
    }

    public partial class CommonParkingCharge
    {
        [JsonProperty("Common.Amount")]
        public double CommonAmount { get; set; }

        [JsonProperty("Common.Interval")]
        public string CommonInterval { get; set; }

        [JsonProperty("Common.CurrencyCode")]
        public string CommonCurrencyCode { get; set; }
    }

    public partial class CommonPointsOfInterest
    {
        [JsonProperty("Common.InterestKind")]
        public string CommonInterestKind { get; set; }

        [JsonProperty("Common.NamesOfPlaces")]
        public List<CommonCommentElement> CommonNamesOfPlaces { get; set; }

        [JsonProperty("Common.Distances")]
        public List<CommonDistance> CommonDistances { get; set; }
    }

    public partial class CommonPointsOfInterests
    {
        [JsonProperty("Common.InterestKind")]
        public string CommonInterestKind { get; set; }

        [JsonProperty("Common.Places")]
        public List<CommonPlace> CommonPlaces { get; set; }
    
    }

    public partial class CommonPlace
    {
        [JsonProperty("Common.Name")]
        public List<CommonCommentElement> CommonName { get; set; }

        [JsonProperty("Common.Type")]
        public List<CommonCommentElement> CommonType { get; set; }

        [JsonProperty("Common.Distances")]
        public List<CommonDistance> CommonDistances { get; set; }

        [JsonProperty("Common.Duration")]
        public List<CommonDuration> CommonDurations { get; set; }
    }
    
    public partial class CommonTransportationTypes
    {
        [JsonProperty("Common.Type")]
        public string CommonType { get; set; }

        [JsonProperty("Common.Places")]
        public List<CommonTransportationPlace> CommonPlaces { get; set; }
    }

    public partial class CommonTransportationPlace
    {
        [JsonProperty("Common.Name")]
        public List<CommonCommentElement> CommonName { get; set; }

        [JsonProperty("Common.Distances")]
        public List<CommonDistance> CommonDistances { get; set; }

        [JsonProperty("Common.Duration")]
        public List<CommonDuration> CommonDurations { get; set; }
    }

    public partial class CommonDistance
    {
        [JsonProperty("Common.DistanceUnits")]
        public string CommonDistanceUnits { get; set; }

        [JsonProperty("Common.Amount")]
        public double CommonAmount { get; set; }
    }

    public partial class CommonDuration
    {
        [JsonProperty("Common.Amount")]
        public double CommonAmount { get; set; }

        [JsonProperty("Common.DistanceUnits")]
        public string CommonDistanceUnits { get; set; }

        [JsonProperty("Common.TravelMode")]
        public string CommonTravelMode { get; set; }
    }

    public partial class CommonSize
    {
        [JsonProperty("Common.SizeKind")]
        public string CommonSizeKind { get; set; }

        [JsonProperty("Common.Dimensions")]
        public List<CommonDimension> CommonDimensions { get; set; }
    }

    public partial class CommonDimension
    {
        [JsonProperty("Common.DimensionsUnits")]
        public string CommonDimensionsUnits { get; set; }

        [JsonProperty("Common.Amount")]
        public double CommonAmount { get; set; }
    }

    public partial class CommonVideoLink
    {
        [JsonProperty("Common.Link")]
        public string CommonLink { get; set; }

        [JsonProperty("Common.CultureCode")]
        public string CommonCultureCode { get; set; }

        [JsonProperty("Common.VideoDescription")]
        public string CommonVideoDescription { get; set; }
    }

    public partial class UnitedKingdomBusinessRatesInfo
    {
        [JsonProperty("UnitedKingdom.RateInThePound")]
        public double UnitedKingdomRateInThePound { get; set; }

        [JsonProperty("UnitedKingdom.RateableValuePounds")]
        public long UnitedKingdomRateableValuePounds { get; set; }
    }

    public partial class CommonSourceSystem
    {
        [JsonProperty("Common.Name")]
        public string CommonName { get; set; }

        [JsonProperty("Common.Id")]
        public string CommonId { get; set; }
    }

    public partial class CommonExternalRatings
    {
        [JsonProperty("Common.RatingType")]
        public string CommonRatingType { get; set; }

        [JsonProperty("Common.RatingLevel")]
        public string CommonRatingLevel { get; set; }

        [JsonProperty("Common.RatingCategory")]
        public string CommonRatingCategory { get; set; }

        [JsonProperty("Common.RatingDate")]
        public string CommonRatingDate { get; set; }

        [JsonProperty("Common.RatingComment")]
        public List<CommonCommentElement> CommonRatingComment { get; set; }
    }

    public partial class PropertyListingEnvelope
    {
        public static PropertyListingEnvelope FromJson(string json) => JsonConvert.DeserializeObject<PropertyListingEnvelope>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this PropertyListingEnvelope self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
