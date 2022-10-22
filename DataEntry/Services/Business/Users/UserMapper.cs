using dataentry.ViewModels.GraphQL;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace dataentry.Services.Business.Users
{
    public class UserMapper : IUserMapper
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserMapper(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager ?? throw new System.ArgumentNullException(nameof(userManager));
        }

        public async Task<UserViewModel> Map(IdentityUser user, UserLookupOptions options)
        {
            var vm = new UserViewModel();

            vm.Id = user.UserName;
            if (options.ShouldQueryUserClaims)
            {
                var claims = await _userManager.GetClaimsAsync(user);
                if (options.IncludeFirstName) vm.FirstName = claims.FirstOrDefault(claim => claim.Type == ClaimTypes.GivenName)?.Value;
                if (options.IncludeLastName) vm.LastName = claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Surname)?.Value;
                if (options.IncludeFullName) vm.FullName = claims.FirstOrDefault(claim => claim.Type == "name")?.Value;
            }

            return vm;
        }
    }
}
