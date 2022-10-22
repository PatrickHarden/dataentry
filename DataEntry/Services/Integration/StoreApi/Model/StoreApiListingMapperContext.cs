using dataentry.Data.Enums;
using dataentry.Utility;
using dataentry.ViewModels.GraphQL;

namespace dataentry.Services.Integration.StoreApi.Model
{
    public class StoreApiListingMapperContext
    {
        public StoreApiListingMapperContext(
            RegionViewModel region,
            string resourcesBaseUrl,
            PropertyListing storeApiListing,
            ListingViewModel dataEntryListing
        )
        {
            Region = region;
            ResourcesBaseUrl = resourcesBaseUrl;
            StoreApiListing = storeApiListing;
            DataEntryListing = dataEntryListing;
        }

        public RegionViewModel Region { get; set; }
        public string ResourcesBaseUrl { get; private set; }
        public PropertyListing StoreApiListing { get; set; }
        public ListingViewModel DataEntryListing { get; set; }
        public PropertyTypeEnum PropertyType { get; set; }
        public AspectsEnum ListingType { get; set; }
        public bool IsFlexRent { get; set; }
        public string CurrencyCode { get;  set; }
        public DimensionsUnitsEnum? DimensionsUnit { get; set; }
    }
}
