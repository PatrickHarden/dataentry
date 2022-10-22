using dataentry.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace dataentry.Middleware
{
    public class CreateUserMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CreateUserMiddleware> _logger;

        public CreateUserMiddleware(
            RequestDelegate next,
            ILogger<CreateUserMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext, ApplicationUserManager userManager)
        {
            var user = await userManager.GetUserAsync(httpContext.User);

            if (user == null && !string.IsNullOrWhiteSpace(httpContext.User.Identity.Name))
            {
                try
                {
                    // Get the users name from claims
                    var userName = httpContext.User.FindFirst(ClaimTypes.Name).Value;
                    user = new IdentityUser(userName)
                    {
                        Id = httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value
                    };

                    var identityResult = await userManager.CreateAsync(user);
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
