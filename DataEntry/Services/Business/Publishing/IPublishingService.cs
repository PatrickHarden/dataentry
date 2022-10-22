using dataentry.ViewModels.GraphQL;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace dataentry.Services.Business.Publishing
{
    public interface IPublishingService
    {
        Task<ListingViewModel> PublishListingAsync(int id, ClaimsPrincipal user);
        Task<ListingViewModel> UnPublishListingAsync(int id, ClaimsPrincipal user);
        Task<ListingViewModel> PreviewListingAsync(int id, ClaimsPrincipal user);
        Task<ListingViewModel> UnPreviewListingAsync(int id, ClaimsPrincipal user);
        Task<ListingViewModel> RunPublishingAsync(PublishingOptions options);
    }
}
