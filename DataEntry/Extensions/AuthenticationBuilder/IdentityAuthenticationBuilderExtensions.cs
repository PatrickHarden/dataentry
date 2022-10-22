using dataentry.Repository;
using dataentry.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace dataentry.Extensions
{
    public static class IdentityAuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddTestIdentity(this AuthenticationBuilder builder)
            => builder.AddTestIdentity(_ => { });

        public static AuthenticationBuilder AddTestIdentity(this AuthenticationBuilder builder, Action<IdentitySettings> configureOptions)
        {
            builder.Services.Configure(configureOptions);
            builder.Services.AddTransient<IConfigureOptions<IdentitySettings>, ConfigureIdentityOptions>();
            return builder;
        }

        private class ConfigureIdentityOptions : IConfigureOptions<IdentitySettings>
        {
            private readonly ApplicationUserManager _userManager;
            private readonly ILogger<ConfigureIdentityOptions> _logger;

            public ConfigureIdentityOptions(
                ApplicationUserManager userManager,
                ILogger<ConfigureIdentityOptions> logger)
            {
                _userManager = userManager;
                _logger = logger;
            }

            public async void Configure(IdentitySettings options)
            {
                try
                {
                    // Try to get test user
                    var user = await _userManager.FindByNameAsync(options.UserName);

                    // Create user if not found
                    if (user == null)
                    {
                        user = new IdentityUser(options.UserName)
                        {
                            Email = options.UserName
                        };

                        var identityResult = await _userManager.CreateAsync(user, options.Password);

                        var claims = new List<Claim>();

                        claims.Add(new Claim("given_name", options.GivenName));
                        claims.Add(new Claim("family_name", options.FamilyName));

                        await _userManager.AddClaimsAsync(user, claims);
                    }

                    // TODO: Update password if user exist
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"Test user failed to get created: {ex.Message}");
                }
            }
        }
    }
}
