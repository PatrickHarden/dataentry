using dataentry.Data.DBContext.Model;
using dataentry.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace dataentry.Services.Integration.Authorization
{
    public class RegionAuthorizationHandler 
        : AuthorizationHandler<OperationAuthorizationRequirement, Region>
    {
        private readonly ApplicationUserManager _userManager;

        public RegionAuthorizationHandler(ApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Region resource)
        {
            if (context.HasSucceeded)
                return;

            var userId = _userManager.GetUserId(context.User);
            
            // Check if user has claim to resource
            if (await _userManager.IsAdminAsync(userId))
                context.Succeed(requirement);
        }
    }
}
