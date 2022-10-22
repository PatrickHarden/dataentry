using dataentry.Data.Constants;
using dataentry.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace dataentry.Services.Integration.Authorization
{
    public class TeamAuthorizationHandler
        : AuthorizationHandler<OperationAuthorizationRequirement, IdentityRole>
    {
        private readonly ApplicationUserManager _userManager;

        public TeamAuthorizationHandler(ApplicationUserManager userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, IdentityRole resource)
        {
            if (context.HasSucceeded)
                return;
                
            var userId = _userManager.GetUserId(context.User);

            // Check that the role is a team
            var roleClaims = await _userManager.GetClaimsAsync(resource);
            if (!roleClaims.Any(claim => claim.Type == UserConstants.TeamRoleClaimName)) return;

            // Get User from User Identity Context
            var user = await _userManager.GetUserAsync(context.User);
            var roles = await _userManager.GetRolesAsync(user);
            var isAdmin = await _userManager.IsAdminAsync(userId);

            // Check if user is in the role
            if (roles.Any(x => x == resource.Name) || isAdmin)
                context.Succeed(requirement);
        }
    }
}
