using dataentry.Publishing.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace dataentry.Publishing.Services
{
    public interface IStorePublishService
    {
        Task<bool> IsListingInDesiredStateAsync(PublishListing publishListing, PublishState desiredState, ILogger log);
    }
}
