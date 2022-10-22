using dataentry.Data.Constants;
using dataentry.ViewModels.GraphQL;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dataentry.Services.Business.Users
{
    public class TeamMapper : ITeamMapper
    {
        public readonly IUserMapper _userMapper;
        public TeamMapper(IUserMapper userMapper)
        {
            _userMapper = userMapper ?? throw new System.ArgumentNullException(nameof(userMapper));
        }
        public async Task<TeamViewModel> Map(IdentityRole team, IEnumerable<IdentityUser> users, UserLookupOptions options)
        {
            var result = Map(team, options);
            var userVMs = new List<UserViewModel>();
            foreach (var user in users)
            {
                userVMs.Add(await _userMapper.Map(user, options));
            }
            result.Users = userVMs;

            return result;
        }

        public TeamViewModel Map(IdentityRole team, UserLookupOptions options)
        {
            var result = new TeamViewModel();
            result.Name = team.Name.StartsWith(UserConstants.TeamRoleNamePrefix) ? team.Name.Substring(UserConstants.TeamRoleNamePrefix.Length) : team.Name;
            return result;
        }
    }
}
