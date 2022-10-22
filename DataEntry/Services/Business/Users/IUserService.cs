using dataentry.Data.DBContext.Model;
using dataentry.ViewModels.GraphQL;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace dataentry.Services.Business.Users
{
    public interface IUserService
    {
        Task<IdentityResult> CreateTeam(string name, IEnumerable<string> userNames);
        Task<IdentityResult> UpdateTeam(ClaimsPrincipal principal, string oldName, string newName, IEnumerable<string> userNames);
        Task<IdentityResult> DeleteTeam(ClaimsPrincipal principal, string name);
        Task<IEnumerable<TeamViewModel>> GetTeams(ClaimsPrincipal principal, UserLookupOptions options);
        Task<IEnumerable<TeamViewModel>> GetTeams(IEnumerable<string> teamNames, UserLookupOptions options);
        Task<IEnumerable<UserViewModel>> SearchUsers(string searchTerm, IEnumerable<string> blacklist, int? skip, int? take, UserLookupOptions options);
        Task<IEnumerable<UserViewModel>> GetUsers(IEnumerable<string> userNames, UserLookupOptions options);
        Task<IEnumerable<ClaimantViewModel>> SearchClaimants(string searchTerm, IEnumerable<string> blacklist, int? skip, int? take, UserLookupOptions options);
    }
}