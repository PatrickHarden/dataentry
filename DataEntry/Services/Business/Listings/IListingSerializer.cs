using dataentry.ViewModels.GraphQL;
using Newtonsoft.Json.Linq;

namespace dataentry.Services.Business.Listings
{
    public interface IListingSerializer
    {
        string Serialize(ListingViewModel vm);
        ListingViewModel Deserialize(string value);
    }
}
