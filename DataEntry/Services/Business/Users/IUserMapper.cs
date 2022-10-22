using dataentry.ViewModels.GraphQL;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace dataentry.Services.Business.Users
{
    public interface IUserMapper
    {
        Task<UserViewModel> Map(IdentityUser user, UserLookupOptions options);
    }
}