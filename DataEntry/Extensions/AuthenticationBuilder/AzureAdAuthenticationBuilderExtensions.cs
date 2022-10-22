using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace dataentry.Extensions
{
    public static class AzureAdAuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddAzureAdBearer(this AuthenticationBuilder builder, Action<AzureAdOptions> configureOptions)
        {
            builder.Services.Configure(configureOptions);
            
            var azureOptions = new AzureAdOptions();
            configureOptions(azureOptions);

            builder.AddJwtBearer(options => {
                options.Audience = azureOptions.ClientId;
                options.Authority = $"{azureOptions.Instance}{azureOptions.TenantId}/v2.0/";
                options.TokenValidationParameters.ValidateIssuer = false;

            });
            return builder;
        }
    }
}
