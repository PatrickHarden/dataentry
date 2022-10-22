using dataentry.Data.DBContext.Model;
using dataentry.Services.Business.Users;
using dataentry.ViewModels.GraphQL;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace dataentry.Services.Business.Listings
{
    public interface IListingService
    {
        Task<ListingViewModel> CreateListingAsync(ClaimsPrincipal user, ListingViewModel vm, bool imported = false);
        Task<ListingViewModel> UpdateListingAsync(ClaimsPrincipal user, ListingViewModel vm);
        Task<bool> DeleteListingAsync(ClaimsPrincipal user, int id);
        Task<ListingViewModel> GetListingById(ClaimsPrincipal user, int id, UserLookupOptions options);
        Task<IEnumerable<SpacesViewModel>> GetEdpSpacesById(ClaimsPrincipal user, int id, UserLookupOptions options);
        Task<IEnumerable<ListingViewModel>> GetListings(ClaimsPrincipal user, int? skip, int? take, UserLookupOptions options, IEnumerable<FilterViewModel>  filterOptionsViewModel, string regionID);
        Task<int> GetListingsCount(ClaimsPrincipal user, UserLookupOptions options, IEnumerable<FilterViewModel>  filterOptionsViewModel, string regionID);
        Task<ContactsViewModel> GetBrokerByEmail(string email);
        Task<IEnumerable<ListingViewModel>> GetEdpImportListings(ClaimsPrincipal user, int id, UserLookupOptions options);
    }
}
