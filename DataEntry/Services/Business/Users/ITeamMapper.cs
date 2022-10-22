using dataentry.Data.DBContext.Model;
using dataentry.ViewModels.GraphQL;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dataentry.Services.Business.Users
{
    public interface ITeamMapper
    {
        Task<TeamViewModel> Map(IdentityRole team, IEnumerable<IdentityUser> users, UserLookupOptions options);
        TeamViewModel Map(IdentityRole team, UserLookupOptions options);
    }
}
