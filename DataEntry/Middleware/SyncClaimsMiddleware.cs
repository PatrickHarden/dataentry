using dataentry.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace dataentry.Middleware
{
    public class SyncClaimsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SyncClaimsMiddleware> _logger;

        private static readonly string[] _claimsToCapture = new[] { ClaimTypes.GivenName, ClaimTypes.Surname, "name" };

        public SyncClaimsMiddleware(
            RequestDelegate next,
            ILogger<SyncClaimsMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext, ApplicationUserManager userManager)
        {
            var user = await userManager.GetUserAsync(httpContext.User);

            if (user != null)
            {
                try
                {
                    var claims = await userManager.GetClaimsAsync(user);
                    foreach (var claimType in _claimsToCapture)
                    {
                        var newClaimValue = httpContext.User.FindFirst(claimType)?.Value;
                        if (newClaimValue == null) continue;

                        var existingClaim = claims.FirstOrDefault(claim => claim.Type == claimType);
                        if (existingClaim == null)
                        {
                            await userManager.AddClaimAsync(user, new Claim(claimType, newClaimValue));
                        }
                        else if (existingClaim?.Value != newClaimValue)
                        {
                            await userManager.ReplaceClaimAsync(user, existingClaim, new Claim(claimType, newClaimValue));
                        }
                    }
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogError(ex.Message);
                }
            } 
            
            await _next(httpContext);
        }
    }
}
