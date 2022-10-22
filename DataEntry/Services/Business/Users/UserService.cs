using dataentry.Repository;
using dataentry.Services.Integration.Authorization;
using dataentry.ViewModels.GraphQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace dataentry.Services.Business.Users
{
    public class UserService : IUserService
    {
        private readonly ApplicationUserManager _userManager;
        private readonly IUserMapper _userMapper;
        private readonly ITeamMapper _teamMapper;
        private readonly IClaimantMapper _claimantMapper;
        private readonly IAuthorizationService _authorizationService;

        public UserService(ApplicationUserManager userManager, IUserMapper userMapper, ITeamMapper teamMapper, IClaimantMapper claimantMapper, IAuthorizationService authorizationService)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _userMapper = userMapper ?? throw new ArgumentNullException(nameof(userMapper));
            _teamMapper = teamMapper ?? throw new ArgumentNullException(nameof(teamMapper));
            _claimantMapper = claimantMapper ?? throw new ArgumentNullException(nameof(claimantMapper));
            _authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
        }

        public async Task<IEnumerable<UserViewModel>> GetUsers(IEnumerable<string> userNames, UserLookupOptions options)
        {
            var users = await _userManager.FindByNamesAsync(userNames);
            var result = new List<UserViewModel>();
            foreach (var user in users)
            {
                result.Add(await _userMapper.Map(user, options));
            }

            return result;
        }

        /// <summary>
        /// Search for the specified user
        /// </summary>
        /// <param name="searchTerm">Search term to search the users with</param>
        /// <param name="blacklist">Optional list of user ids to exclude from the search results</param>
        /// <param name="skip">Number of users to remove from the beginning of the result set</param>
        /// <param name="take">Maximum number of users to return</param>
        /// <returns>The search results</returns>
        public async Task<IEnumerable<UserViewModel>> SearchUsers(string searchTerm, IEnumerable<string> blacklist, int? skip, int? take, UserLookupOptions options)
        {
            var users = await _userManager.FindBySearchTermAsync(searchTerm, blacklist, skip, take);
            var result = new List<UserViewModel>();
            foreach (var user in users)
            {
                result.Add(await _userMapper.Map(user, options));
            }
            return result;
        }

        /// <summary>
        /// Get all teams
        /// </summary>
        /// <param name="principal">If not null, result set will only contain teams that current login user has access to</param>
        /// <param name="skip">Number of teams to remove from the beginning of the result set</param>
        /// <param name="take">Maximum number of teams to return</param>
        /// <returns>All teams</returns>
        public async Task<IEnumerable<TeamViewModel>> GetTeams(ClaimsPrincipal principal, UserLookupOptions options)
        {
            IdentityUser user = null;
            if (principal != null)
            {
                user = await _userManager.GetUserAsync(principal);
            }
            return await GetTeams(user, options);
        }


        /// <summary>
        /// Get all teams
        /// </summary>
        /// <param name="user">If not null, result set will only contain teams that this user has access to</param>
        /// <param name="skip">Number of teams to remove from the beginning of the result set</param>
        /// <param name="take">Maximum number of teams to return</param>
        /// <returns>All teams</returns>
        private async Task<IEnumerable<TeamViewModel>> GetTeams(IdentityUser user, UserLookupOptions options)
        {
            var teamNames = await _userManager.GetTeamsAsync(user);
            return await GetTeams(teamNames, options);
        }

        public async Task<IEnumerable<TeamViewModel>> GetTeams(IEnumerable<string> teamNames, UserLookupOptions options)
        {
            var teams = await _userManager.FindRolesByTeamNamesAsync(teamNames);
            return await GetTeams(teams, options);
        }

        private async Task<IEnumerable<TeamViewModel>> GetTeams(IEnumerable<IdentityRole> roles, UserLookupOptions options)
        {
            var result = new List<TeamViewModel>();
            foreach (var role in roles)
            {
                if (options.IncludeTeamMembers)
                {
                    var users = await _userManager.GetUsersInRoleAsync(role.Name);
                    result.Add(await _teamMapper.Map(role, users, options));
                } else
                {
                    result.Add(_teamMapper.Map(role, options));
                }
            }
            return result;
        }

        /// <summary>
        /// Creates a team
        /// </summary>
        /// <param name="name">The name of the team</param>
        /// <param name="userNames">A list of users in the team</param>
        /// <returns>IdentityResult object</returns>
        public async Task<IdentityResult> CreateTeam(string name, IEnumerable<string> userNames)
        {
            var users = await _userManager.FindByNamesAsync(userNames);
            var result = await _userManager.CreateTeamAsync(name, users);

            return result;
        }

        /// <summary>
        /// Update a team
        /// </summary>
        /// <param name="principal">The user attempting to make the change</param>
        /// <param name="oldName">The original name of the team</param>
        /// <param name="newName">The new name of the team. It can match the old name to indicate no change.</param>
        /// <param name="userNames">The updated list of user names. It can match the original list of users to indicate no change.</param>
        /// <returns>IdentityResult object</returns>
        public async Task<IdentityResult> UpdateTeam(ClaimsPrincipal principal, string oldName, string newName, IEnumerable<string> userNames)
        {
            var users = await _userManager.FindByNamesAsync(userNames);
            var role = await _userManager.FindRoleByTeamNameAsync(oldName);

            var authResult = await _authorizationService.AuthorizeAsync(principal, role, Operations.Update);
            if (!authResult.Succeeded)
            {
                return IdentityResult.Failed(new IdentityError { Code = "NotAuthorized", Description = $"User \"{principal.Identity.Name}\" does not have access to Role \"{role.Name}\"." });
            }

            var result = await _userManager.UpdateTeamAsync(principal, role, newName, users);

            return result;
        }


        /// <summary>
        /// Delete a team
        /// </summary>
        /// <param name="principal">The user attempting to delete the team</param>
        /// <param name="name">The name of the team</param>
        /// <returns>IdentityResult object</returns>
        public async Task<IdentityResult> DeleteTeam(ClaimsPrincipal principal, string name)
        {
            var role = await _userManager.FindRoleByTeamNameAsync(name);

            var authResult = await _authorizationService.AuthorizeAsync(principal, role, Operations.Delete);
            if (!authResult.Succeeded)
            {
                return IdentityResult.Failed(new IdentityError { Code = "NotAuthorized", Description = $"User \"{principal.Identity.Name}\" does not have access to Role \"{role.Name}\"." });
            }

            var result = await _userManager.DeleteAsync(role);

            return result;
        }

        public async Task<IEnumerable<ClaimantViewModel>> SearchClaimants(string searchTerm, IEnumerable<string> blacklist, int? skip, int? take, UserLookupOptions options)
        {
            var claimants = await _userManager.FindClaimantsBySearchTermAsync(searchTerm, blacklist, skip, take);
            var result = new List<ClaimantViewModel>();
            foreach (var claimant in claimants)
            {
                result.Add(_claimantMapper.Map(claimant));
            }
            return result;
        }

    }
}
