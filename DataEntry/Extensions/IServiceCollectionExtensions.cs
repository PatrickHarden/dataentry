using dataentry.Middleware;
using dataentry.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace dataentry.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static OptionsBuilder<AdminOptions> AddAdminOptions(this IServiceCollection services)
        {
            return services.AddOptions<AdminOptions>().Configure<IConfiguration>((options, configuration) =>
            {
                // Support appsettings.json complex object
                // Example: "Admins":{"global":["a@a.com","b@b.com"], "us-comm":["c@c.com","d@d.com"]}
                configuration.Bind("Admins", options);

                // Support legacy Admins environment variable
                // Example: "Admins": "a@a.com;b@b.com"
                var section = configuration.GetSection("Admins");
                if (!string.IsNullOrWhiteSpace(section.Value))
                {
                    BuildUserSet(options, "global", section.Value);
                }

                // Support regional environment variable
                // Example: 
                //  "Admins__global": "a@a.com;b@b.com"
                //  "Admins__us-comm": "c@c.com;d@d.com"
                foreach (var regionSection in section.GetChildren())
                {
                    if (!string.IsNullOrWhiteSpace(regionSection.Value))
                    {
                        BuildUserSet(options, regionSection.Key, regionSection.Value);
                    }
                }
            });
        }

        private static void BuildUserSet(AdminOptions options, string key, string users)
        {
            if (!options.TryGetValue(key, out var userSet))
            {
                userSet = new CaseInsensitiveHashSet();
                options[key] = userSet;
            }

            foreach (var user in users.Split(';'))
            {
                userSet.Add(user);
            }
        }
    }
}