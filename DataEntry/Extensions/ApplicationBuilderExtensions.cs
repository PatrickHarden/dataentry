using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dataentry.Data.Constants;
using dataentry.Data.DBContext;
using dataentry.Data.DBContext.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace dataentry.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseDefaultRegionConfigValues(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            using var context = app.ApplicationServices.GetRequiredService<DataEntryContext>();

            var defaultRegion = context.Regions.Find(Region.DefaultID);
            if (defaultRegion?.Name == "Default") // Value set by migration 20200826211432_Regions
            {
                app.ApplicationServices.GetService<IConfiguration>().Bind("DefaultRegion", defaultRegion);
                context.SaveChanges();
            }
        }

        public static void UseAdminRoles(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            using var dataEntryContext = scope.ServiceProvider.GetRequiredService<DataEntryContext>();
            using var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var tasks = new List<Task>();

            BuildAdminRole(UserConstants.AdminRoleName, UserConstants.AdminClaimType, true.ToString(), roleManager).Wait();
            foreach (var region in dataEntryContext.Regions)
            {
                BuildAdminRole(string.Format(UserConstants.RegionAdminRoleFormat, region.ID), UserConstants.RegionAdminClaimType, region.ID.ToString(), roleManager).Wait();
            }
        }

        private static async Task BuildAdminRole(string roleName, string claimType, string claimValue, RoleManager<IdentityRole> roleManager)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                role = new IdentityRole(roleName);
                await roleManager.CreateAsync(role);
            }

            var roleClaims = await roleManager.GetClaimsAsync(role);
            if (!roleClaims.Any(c => c.Type == claimType && c.Value == claimValue))
            {
                await roleManager.AddClaimAsync(role, new System.Security.Claims.Claim(claimType, claimValue));
            }
        }
    }
}