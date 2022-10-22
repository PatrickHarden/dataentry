using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dataentry.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace dataentry.Middleware
{
    public class AddAdminMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AdminOptions _admins;
        private readonly ILogger<AddAdminMiddleware> _logger;

        public AddAdminMiddleware(
            RequestDelegate next,
            ApplicationUserManager userManager,
            ILogger<AddAdminMiddleware> logger,
            IOptions<AdminOptions> options)
        {
            _next = next;
            _logger = logger;
            _admins = options.Value;
        }

        public async Task Invoke(HttpContext httpContext, ApplicationUserManager userManager)
        {
            // This gets called 5+ times per click on the front end, so use session state to limit 
            // the number of calls to once per 5 min
            var lastRun = httpContext.Session.GetString("AdminCheck");
            var now = DateTime.UtcNow;
            if (lastRun == null ||
                !DateTime.TryParse(lastRun, out var lastRunTime) ||
                now - lastRunTime > new TimeSpan(0, 5, 0))
            {
                var user = await userManager.GetUserAsync(httpContext.User);

                if (user != null)
                {
                    foreach (var kv in _admins)
                    {
                        using (_logger.BeginScope(new Dictionary<string, object>{
                            ["AddAdminMiddleware_Region"] = kv.Key,
                            ["AddAdminMiddleware_User"] = kv.Value
                        }))
                        try
                        {
                            if (kv.Value.Contains(user.UserName))
                            {
                                if (kv.Key.Equals("global", StringComparison.OrdinalIgnoreCase))
                                {
                                    await userManager.AddAdminAsync(user);
                                }
                                else
                                {
                                    await userManager.AddRegionAdminByHomeSiteIDAsync(user, kv.Key.Replace("_","-"));
                                }
                            }
                        }
                        catch (InvalidOperationException ex)
                        {
                            _logger.LogError(ex.Message);
                        }
                    }

                    httpContext.Session.SetString("AdminCheck", now.ToString());
                }
            }

            await _next(httpContext);
        }
    }
}