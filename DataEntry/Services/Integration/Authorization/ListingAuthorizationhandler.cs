using dataentry.Data.Constants;
using dataentry.Data.DBContext.Model;
using dataentry.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace dataentry.Services.Integration.Authorization
{
    public class ListingAuthorizationHandler 
        : AuthorizationHandler<OperationAuthorizationRequirement, Listing>
    {
        private readonly ApplicationUserManager _userManager;

        public ListingAuthorizationHandler(ApplicationUserManager userManager, ILogger<ListingAuthorizationHandler> logger)
        {
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Listing resource)
        {
            if (context.HasSucceeded)
                return;

            if (resource == null)
                return;

            if (requirement.Name == Operations.Read.Name || requirement.Name == Operations.Update.Name) {
                // Allowing janitors to edit any listing ¯\_(ツ)_/¯
                context.Succeed(requirement);
                return;
            }

            var listingId = resource.ID.ToString();
            var regionId = resource.RegionID.ToString();
            var userId = _userManager.GetUserId(context.User);

            // Check if user has claim to resource
            if (await _userManager.HasClaimAsync(userId, (claimType, claimValue) => 
                claimType == UserConstants.ListingClaimName && claimValue == listingId
                || claimType == UserConstants.AdminClaimType
                || claimType == UserConstants.RegionAdminClaimType && claimValue == regionId))
                context.Succeed(requirement);
        }
    }
}
