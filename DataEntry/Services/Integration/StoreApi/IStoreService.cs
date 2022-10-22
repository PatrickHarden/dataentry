using dataentry.Services.Integration.StoreApi.Model;
using System.Threading.Tasks;

namespace dataentry.Services.Integration.StoreApi
{
    public interface IStoreService
    {
        Task<dynamic> AddListing(PropertyListing propertyListing);

        Task<string> GetListing(PropertyListing propertyListing);

        Task<string> RemoveListing(string primaryKey);

        Task<string> HealthCheck();
    }
}
