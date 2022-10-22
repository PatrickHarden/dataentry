using dataentry.Data.Constants;
using dataentry.Data.DBContext;
using dataentry.Data.DBContext.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace dataentry.Repository
{
    public class ApplicationUserManager : UserManager<IdentityUser>
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DataEntryContext _dataEntryContext;

        private ApplicationUserStore _store => Store as ApplicationUserStore ?? throw new NotSupportedException($"Expected {nameof(ApplicationUserStore)}. Store is of type {Store.GetType().FullName}");

        public ApplicationUserManager(
            IUserStore<IdentityUser> store, 
            IOptions<IdentityOptions> optionsAccessor, 
            IPasswordHasher<IdentityUser> passwordHasher, 
            IEnumerable<IUserValidator<IdentityUser>> userValidators, 
            IEnumerable<IPasswordValidator<IdentityUser>> passwordValidators, 
            ILookupNormalizer keyNormalizer, 
            IdentityErrorDescriber errors, 
            IServiceProvider services, 
            ILogger<UserManager<IdentityUser>> logger,
            RoleManager<IdentityRole> roleManager,
            DataEntryContext dataEntryContext) 
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _roleManager = roleManager;
            _dataEntryContext = dataEntryContext;
        }

        public async Task<bool> HasClaimAsync(string userId, Expression<Func<string, string, bool>> condition)
            => await _store.HasClaimAsync(userId, condition);

        /// <summary>
        /// Gets a list of users by ID
        /// </summary>
        /// <param name="userNames">The IDs of the users to return</param>
        /// <returns>The users</returns>
        public async Task<IEnumerable<IdentityUser>> FindByNamesAsync(IEnumerable<string> userNames)
        {
            ThrowIfDisposed();
            if (userNames == null) return new List<IdentityUser>();
            userNames = userNames.Select(x => NormalizeName(x));
            return await _store.FindByNameAsync(userNames, CancellationToken);
        }

        /// <summary>
        /// Search for the specified user
        /// </summary>
        /// <param name="searchTerm">Search term to search the users with</param>
        /// <param name="blacklist">Optional list of user ids to exclude from the search results</param>
        /// <param name="skip">Number of users to remove from the beginning of the result set</param>
        /// <param name="take">Maximum number of users to return</param>
        /// <returns>The search results</returns>
        public async Task<IEnumerable<IdentityUser>> FindBySearchTermAsync(string searchTerm, IEnumerable<string> blacklist, int? skip, int? take)
        {
            ThrowIfDisposed();
            if (string.IsNullOrWhiteSpace(searchTerm)) return new List<IdentityUser>();
            var normalizedSearchTerm = NormalizeName(searchTerm);
            var normalizedBlacklist = blacklist.Select(x => NormalizeName(x));
            return await _store.FindBySearchTermAsync(normalizedSearchTerm, normalizedBlacklist, skip, take, CancellationToken);
        }

        /// <summary>
        /// Search users and teams (claimants)
        /// 
        /// TODO: EF Core LINQ to entities does not support converting all of the LINQ functionality used in this method into SQL, resulting in 
        /// running a lot of the logic in memory. This is really bad for scalability as it will load the entire user/role db into memory. Either 
        /// refactor to supported linq functions (probably impossible in our current EF version) or convert to raw sql or a stored proc.
        /// </summary>
        /// <param name="searchTerm">Search term to search the claimants with. Searches user id, user first name, user last name, and team name</param>
        /// <param name="blacklist">Optional list of user ids or team names to exclude from the search results</param>
        /// <param name="skip">Number of claimants to remove from the beginning of the result set</param>
        /// <param name="take">Maximum number of claimants to return</param>
        /// <returns>The search results</returns>
        public async Task<IEnumerable<Claimant>> FindClaimantsBySearchTermAsync(string searchTerm, IEnumerable<string> blacklist, int? skip, int? take)
        {
            ThrowIfDisposed();
            var normalizedSearchTerm = searchTerm == null ? null : NormalizeName(searchTerm);
            var normalizedBlacklist = blacklist?.Select(userName => NormalizeName(userName)).ToList();
            return await _store.FindClaimantsBySearchTermAsync(normalizedSearchTerm, normalizedBlacklist, skip, take, CancellationToken);
        }

        public async Task<IdentityRole> FindRoleByTeamNameAsync(string teamName)
        {
            ThrowIfDisposed();
            if (teamName == null) return null;
            var normalizedRoleName = NormalizeName(ConvertTeamNameToRoleName(teamName));
            return await _roleManager.FindByNameAsync(normalizedRoleName);
        }

        public async Task<IEnumerable<IdentityRole>> FindRolesByTeamNamesAsync(IEnumerable<string> teamNames)
        {
            ThrowIfDisposed();
            if (teamNames == null) return new List<IdentityRole>();
            var normalizedTeamNames = teamNames.Select(x => NormalizeName(ConvertTeamNameToRoleName(x)));
            return await _store.FindRolesByNamesAsync(normalizedTeamNames, CancellationToken);
        }

        public string ConvertTeamNameToRoleName(string teamName)
        {
            ThrowIfDisposed();
            var normalizedTeamName = NormalizeName(teamName);
            var normalizedTeamRoleNamePrefix = NormalizeName(UserConstants.TeamRoleNamePrefix);
            return normalizedTeamName.StartsWith(normalizedTeamRoleNamePrefix) ? teamName : UserConstants.TeamRoleNamePrefix + teamName;
        }

        /// <summary>
        /// Get all teams
        /// </summary>
        /// <param name="user">If not null, result set will only contain teams that this user has access to</param>
        /// <returns>All teams</returns>
        public async Task<IEnumerable<IdentityRole>> GetTeamsAsync(IdentityUser user)
        {
            ThrowIfDisposed();
            return await _store.GetTeamsAsync(user, CancellationToken);
        }

        public async Task<IdentityResult> CreateTeamAsync(string name, IEnumerable<IdentityUser> users)
        {
            ThrowIfDisposed();
            var roleName = ConvertTeamNameToRoleName(name);
            if (users == null) users = new List<IdentityUser>();

            //Capture all errors in this list
            var errors = new List<IdentityError>();

            var role = new IdentityRole(roleName);
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    if (error.Code == "DuplicateRoleName")
                    {
                        errors.Add(new IdentityError { Code = "DuplicateTeamName", Description = $"Team name '{name}' is already taken." });
                    }
                    else
                    {
                        errors.Add(error);
                    }
                }
            }
            else
            {
                //Add team claim
                errors.AddRange((await AddClaimAsync(role, new Claim(UserConstants.TeamRoleClaimName, role.Id))).Errors);

                //Add users to role
                foreach (var user in users)
                {
                    errors.AddRange((await AddToRoleAsync(user, roleName)).Errors);
                }

                if (errors.Any())
                {
                    //Attempt to roll back
                    errors.AddRange((await _roleManager.DeleteAsync(role)).Errors);
                }
            }

            if (errors.Any())
            {
                //Return the errors
                return IdentityResult.Failed(errors.ToArray());
            }

            return IdentityResult.Success;
        }

        /// <summary>
        /// Update a team
        /// </summary>
        /// <param name="principal">The user attempting to make the change</param>
        /// <param name="oldName">The original name of the team</param>
        /// <param name="newName">The new name of the team. It can match the old name to indicate no change.</param>
        /// <param name="users">The updated list of users. It can match the original list of users to indicate no change.</param>
        /// <returns>IdentityResult object</returns>
        public async Task<IdentityResult> UpdateTeamAsync(ClaimsPrincipal principal, IdentityRole role, string newName, IEnumerable<IdentityUser> users)
        {
            ThrowIfDisposed();
            var newRoleName = ConvertTeamNameToRoleName(newName);
            var newRoleNameNormalized = NormalizeName(newName);
            if (users == null) users = new List<IdentityUser>();

            //Capture all errors in this list
            var errors = new List<IdentityError>();

            if (role.Name != newRoleName)
            {
                role.Name = newRoleName;
                role.NormalizedName = newRoleNameNormalized;
                var result = await _roleManager.UpdateAsync(role);
                if (!result.Succeeded) return result;
            }

            //Remove users from role
            var existingUsers = await GetUsersInRoleAsync(role.Name);
            foreach (var user in existingUsers)
            {
                if (!users.Contains(user))
                {
                    errors.AddRange((await RemoveFromRoleAsync(user, role.Name)).Errors);
                }
            }

            //Add users to role
            foreach (var user in users)
            {
                if (!await IsInRoleAsync(user, role.Name))
                {
                    errors.AddRange((await AddToRoleAsync(user, role.Name)).Errors);
                }
            }

            if (errors.Any()) return IdentityResult.Failed(errors.ToArray());
            else return IdentityResult.Success;
        }

        public async Task<IEnumerable<IdentityUser>> GetUsersForListingAsync(int listingId)
        {
            ThrowIfDisposed();
            var listingClaim = new Claim(UserConstants.ListingClaimName, listingId.ToString());
            return await GetUsersForClaimAsync(listingClaim);
        }

        public async Task<IEnumerable<IdentityRole>> GetRolesForListingAsync(int listingId)
        {
            ThrowIfDisposed();
            var listingClaim = new Claim(UserConstants.ListingClaimName, listingId.ToString());
            return await _store.GetRolesForClaimAsync(listingClaim, CancellationToken); 
        }

        public async Task<IdentityUser> GetListingOwnerAsync(int listingId)
        {
            ThrowIfDisposed();
            var listingClaim = new Claim(UserConstants.OwnerClaimName, listingId.ToString());
            var users = await GetUsersForClaimAsync(listingClaim);
            return users.FirstOrDefault();
        }

        public async Task<IdentityResult> AddClaimAsync(IdentityRole role, Claim claim) => await _roleManager.AddClaimAsync(role, claim);
        public async Task<IdentityResult> RemoveClaimAsync(IdentityRole role, Claim claim) => await _roleManager.RemoveClaimAsync(role, claim);
        public async Task<IList<Claim>> GetClaimsAsync(IdentityRole role) => await _roleManager.GetClaimsAsync(role);
        public async Task<IdentityRole> FindRoleByNameAsync(string roleName) => await _roleManager.FindByNameAsync(roleName);
        public async Task<IdentityResult> DeleteAsync(IdentityRole role) => await _roleManager.DeleteAsync(role);
        public async Task<bool> RoleExistsAsync(string roleName) => await _roleManager.RoleExistsAsync(roleName);
        public async Task<IdentityResult> CreateAsync(IdentityRole role) => await _roleManager.CreateAsync(role);

        /// <summary>
        /// Returns true if the user is an admin. Regional admin status is ignored. 
        /// </summary>
        /// <param name="principal">User to check</param>
        /// <returns>True if the user is an admin</returns>
        public async Task<bool> IsAdminAsync(string userId)
        {
            return await HasClaimAsync(userId, (claimType, claimValue) => 
                claimType == UserConstants.AdminClaimType);
        }

        /// <summary>
        /// Returns true if the user is an admin in the given region. 
        /// Non-regional admins are considered admins in all regions.
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="regionID"></param>
        /// <returns></returns>
        public async Task<bool> IsAdminInRegionAsync(string userId, string regionID)
        {
            return await HasClaimAsync(userId, (claimType, claimValue) => 
                claimType == UserConstants.AdminClaimType 
                || (claimType == UserConstants.RegionAdminClaimType
                    && claimValue == regionID));
        }
        
        public async Task<IdentityResult> AddAdminAsync(IdentityUser user)
        {
            if (!(await IsInRoleAsync(user, UserConstants.AdminRoleName)))
            {
                return await AddToRoleAsync(user, UserConstants.AdminRoleName);
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> AddRegionAdminByHomeSiteIDAsync(IdentityUser user, string homeSiteID)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrEmpty(homeSiteID))
            {
                throw new ArgumentException($"'{nameof(homeSiteID)}' cannot be null or empty.", nameof(homeSiteID));
            }

            using (Logger.BeginScope(new Dictionary<string,object>{
                ["ApplicationUserManager_userName"] = user.UserName,
                ["ApplicationUserManager_homeSiteID"] = homeSiteID
            }))
            {
                Logger.LogDebug("ApplicationUserManager.");

                var region = await _dataEntryContext.Regions.FirstOrDefaultAsync(r => r.HomeSiteID.ToLower() == homeSiteID.ToLower());
                if (region == null)
                {
                    var error = new IdentityError{Code = "InvalidRegion", Description = $"Region does not exist with homesite: {homeSiteID}"};
                    Log(LogLevel.Warning, error);
                    return IdentityResult.Failed(error);
                }
                
                var roleName = string.Format(UserConstants.RegionAdminRoleFormat, region?.ID);
                
                if (!(await IsInRoleAsync(user, roleName)))
                {
                    return await AddToRoleAsync(user, roleName);
                }

                return IdentityResult.Success;
            }
        }

        private void Log(LogLevel logLevel, IdentityError error)
        {
            Logger.Log(logLevel, "({ApplicationUserManager_errorCode}) {ApplicationUserManager_errorDescription}", error.Code, error.Description);
        }
    }
}
