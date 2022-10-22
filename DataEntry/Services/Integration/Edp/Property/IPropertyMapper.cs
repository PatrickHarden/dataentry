using dataentry.Data.DBContext.Model;
using dataentry.Services.Integration.Edp.Consumption;
using dataentry.ViewModels.GraphQL;

namespace dataentry.Services.Integration.Edp
{
    public interface IPropertyMapper
    {
        PropertySearchResultViewModel Map(PropertyResult property);
        ListingViewModel ConvertToListing(PropertyWithAvailability property, Region region);
    }
}
