using dataentry.Data.DBContext.Model;

namespace dataentry.Services.Integration.StoreApi.Model
{
    public interface IListingAdapter
    {
        PropertyListing ConvertToPropertyListing(Listing listing);

        PropertyListing ConvertToPreviewPropertyListing(Listing listing);
    }
}
