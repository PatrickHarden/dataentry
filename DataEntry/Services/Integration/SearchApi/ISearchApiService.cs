using dataentry.ViewModels.GraphQL;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace dataentry.Services.Integration.SearchApi
{
    public interface ISearchApiService
    {
        Task<ImportListingViewModel> ImportListingsByQuery(
            ClaimsPrincipal user,
            string homeSiteId,
            string query,
            bool overwrite,
            List<string> assignToUsers,
            List<string> assignToTeams
        );
    }
}
