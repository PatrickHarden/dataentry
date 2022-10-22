using dataentry.AutoGraph;
using dataentry.Extensions;
using dataentry.Repository;
using dataentry.Services.Business.Configs;
using dataentry.Services.Business.Contacts;
using dataentry.Services.Business.Images;
using dataentry.Services.Business.Listings;
using dataentry.Services.Business.Regions;
using dataentry.Services.Business.Users;
using dataentry.Services.Integration.Edp.Consumption;
using GraphQL.Language.AST;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace dataentry.ViewModels.GraphQL
{
    public class DataEntryQuery : ScopedObjectGraphType
    {
        public DataEntryQuery(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            FieldAsync<AutoObjectGraphType<ListingViewModel>>(
                name: "listing",
                description: "Used to get a listing by id",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" }),
                resolve: async context =>
                {
                    var user = (ClaimsPrincipal)context.UserContext;
                    var id = context.GetArgument<int>("id");
                    var listingService = base.GetService<IListingService>();

                    var options = new UserLookupOptions();
                    GetUserLookupOptionsForListings(options, context);

                    return await listingService.GetListingById(user, id, options);
                });

            FieldAsync<ListGraphType<AutoObjectGraphType<ListingViewModel>>>(
                "listings",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "skip"}, 
                    new QueryArgument<IntGraphType> { Name = "take"},
                    new QueryArgument<ListGraphType<AutoInputObjectGraphType<FilterViewModel>>> { Name = "filterOptions" },
                    new QueryArgument<StringGraphType> { Name = "regionID" }),                   
                resolve: async context =>
                {
                    var user = (ClaimsPrincipal)context.UserContext;
                    var skip = context.GetArgument<int?>("skip");
                    var take = context.GetArgument<int?>("take");
                    var filterOptions = context.GetArgument<IEnumerable<FilterViewModel>>("filterOptions");
                    var regionID = context.GetArgument<string>("regionID");
                    
                    var listingService = base.GetService<IListingService>();
                    var options = new UserLookupOptions();
                    GetUserLookupOptionsForListings(options, context);

                    return await listingService.GetListings(user, skip, take, options, filterOptions, regionID);
                });

            FieldAsync<ListGraphType<AutoObjectGraphType<ListingViewModel>>>(
                "getEdpImportProperty",
                description: "Get EDP Property Details by property id",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" },
                    new QueryArgument<StringGraphType> { Name = "regionID" }),
                resolve: async context =>
                {
                    var user = (ClaimsPrincipal)context.UserContext;
                    var id = context.GetArgument<int>("id");
                    var regionID = new Guid(context.GetArgument<string>("regionID"));

                    var options = new UserLookupOptions();
                    GetUserLookupOptionsForListings(options, context);

                    var edpGraphQLService = base.GetService<IConsumptionService>();
                    var edpProperty = await edpGraphQLService.GetListingByMiqId(id, regionID, true);
                    var listingService = base.GetService<IListingService>();
                    var results = await listingService.GetEdpImportListings(user, id, options);

                    if (edpProperty != null) results = results.Prepend(edpProperty);
                    return results;
                });
            
            FieldAsync<ListGraphType<AutoObjectGraphType<SpacesViewModel>>>(
                name: "spaces",
                description: "Used to get spaces by id, combine with miq availabilities",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" }),
                resolve: async context =>
                {
                    var user = (ClaimsPrincipal)context.UserContext;
                    var id = context.GetArgument<int>("id");
                    var listingService = base.GetService<IListingService>();

                    var options = new UserLookupOptions();
                    GetUserLookupOptionsForListings(options, context);

                    return await listingService.GetEdpSpacesById(user, id, options);
                });
            
            FieldAsync<IntGraphType>(
                "count",
                arguments: new QueryArguments(
                    new QueryArgument<ListGraphType<AutoInputObjectGraphType<FilterViewModel>>> { Name = "filterOptions" },
                    new QueryArgument<StringGraphType> { Name = "regionID" }),
                resolve: async context =>
                {
                    var user = (ClaimsPrincipal)context.UserContext;
                    var filterOptions = context.GetArgument<IEnumerable<FilterViewModel>>("filterOptions");
                    
                    var listingService = base.GetService<IListingService>();
                    var options = new UserLookupOptions();
                    var regionID = context.GetArgument<string>("regionID");

                    return await listingService.GetListingsCount(user, options, filterOptions, regionID);
                });

            FieldAsync<ListGraphType<AutoObjectGraphType<ContactsViewModel>>>(
                "contacts",
                resolve: async context =>
                {
                    var contactService = base.GetService<IContactService>();
                    return await contactService.GetAllBrokers();
                });

            FieldAsync<AutoObjectGraphType<ConfigsViewModel>>(
                "configs",
                description: "Used to get a site configurations",
                resolve: async context =>
                {
                    var configService = base.GetService<IConfigService>();
                    return await configService.GetConfigs();
                });

            FieldAsync<ListGraphType<AutoObjectGraphType<UserViewModel>>>(
                "users",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "term" },
                    new QueryArgument<ListGraphType<StringGraphType>> { Name = "blacklist" },
                    new QueryArgument<IntGraphType> { Name = "skip" },
                    new QueryArgument<IntGraphType> { Name = "take" }),
                resolve: async context =>
                {
                    var userService = base.GetService<IUserService>();
                    var searchTerm = context.GetArgument<string>("term");
                    var blacklist = context.GetArgument<IEnumerable<string>>("blacklist");
                    var skip = context.GetArgument<int?>("skip");
                    var take = context.GetArgument<int?>("take");

                    var options = new UserLookupOptions();
                    GetUserLookupOptionsForUsers(options, context);

                    return await userService.SearchUsers(searchTerm, blacklist, skip, take, options);
                });

            FieldAsync<ListGraphType<AutoObjectGraphType<TeamViewModel>>>(
                "teams",
                resolve: async context =>
                {
                    var userService = base.GetService<IUserService>();
                    var user = (ClaimsPrincipal)context.UserContext;
                    var options = new UserLookupOptions();
                    GetUserLookupOptionsForTeams(options, context);

                    return await userService.GetTeams(user, options);
                });

            FieldAsync<ListGraphType<AutoObjectGraphType<ClaimantViewModel>>>(
                "claimants",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "term" },
                    new QueryArgument<ListGraphType<StringGraphType>> { Name = "blacklist" },
                    new QueryArgument<IntGraphType> { Name = "skip" },
                    new QueryArgument<IntGraphType> { Name = "take" }),
                resolve: async context =>
                {
                    var userService = base.GetService<IUserService>();
                    var searchTerm = context.GetArgument<string>("term");
                    var blacklist = context.GetArgument<IEnumerable<string>>("blacklist");
                    var skip = context.GetArgument<int?>("skip");
                    var take = context.GetArgument<int?>("take");

                    var options = new UserLookupOptions();
                    GetUserLookupOptionsForUsers(options, context);

                    return await userService.SearchClaimants(searchTerm, blacklist, skip, take, options);
                });

            FieldAsync<ListGraphType<AutoObjectGraphType<ImageDetectionViewModel>>>(
                "images",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "listingId" },
                    new QueryArgument<ListGraphType<IntGraphType>> { Name = "imageIds" }
                ),
                resolve: async context =>
                {
                    var user = (ClaimsPrincipal)context.UserContext;
                    var listingId = context.GetArgument<int?>("listingId");
                    var imageIds = context.GetArgument<List<int?>>("imageIds");
                    var imageService = base.GetService<IImageService>();

                    return await imageService.GetImages(listingId, imageIds, user);
                });
            FieldAsync<BooleanGraphType>(
                "isSuperAdmin",
                resolve: async context =>
                {
                    var user = (ClaimsPrincipal)context.UserContext;
                    var userManager = base.GetService<ApplicationUserManager>();
                    var userId = userManager.GetUserId(user);
                    return await userManager.IsAdminAsync(userId);
                });

            FieldAsync<BooleanGraphType>(
                "isAdmin",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "regionID" }
                ),
                resolve: async context =>
                {
                    var user = (ClaimsPrincipal)context.UserContext;
                    var regionID = context.GetArgument<string>("regionID");
                    var userManager = base.GetService<ApplicationUserManager>();
                    var userId = userManager.GetUserId(user);
                    bool isAdmin = await userManager.IsAdminInRegionAsync(userId, regionID);
                    return isAdmin;
                });

            FieldAsync<ListGraphType<AutoObjectGraphType<RegionViewModel>>>(
                "regions",
                resolve: async context =>
                {
                    var regionService = base.GetService<IRegionService>();
                    return await regionService.GetRegions();
                });

            FieldAsync<ListGraphType<AutoObjectGraphType<PropertySearchResultViewModel>>>(
                "searchEdpProperties",
                description: "Search EDP properties by property name",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "keyword" },
                    new QueryArgument<StringGraphType> { Name = "country" }),
                resolve: async context =>
                {
                    var user = (ClaimsPrincipal)context.UserContext;
                    var keyword = context.GetArgument<string>("keyword");
                    var country = context.GetArgument<string>("country");

                    var edpGraphQLService = base.GetService<IConsumptionService>();
                    return await edpGraphQLService.SearchProperties(keyword, country);
                });
            
            FieldAsync<AutoObjectGraphType<ListingViewModel>>(
                "getPropertyDetail",
                description: "Get EDP Property Details by property id",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" },
                    new QueryArgument<StringGraphType> { Name = "regionID" }),
                resolve: async context =>
                {
                    var user = (ClaimsPrincipal)context.UserContext;
                    var id = context.GetArgument<int>("id");
                    var regionID = new Guid(context.GetArgument<string>("regionID"));

                    var edpGraphQLService = base.GetService<IConsumptionService>();
                    return await edpGraphQLService.GetListingByMiqId(id, regionID, true);
                });
        }

        private static void GetUserLookupOptionsForListings(UserLookupOptions options, ResolveFieldContext<object> context)
        {
            if (context?.SubFields == null) return;

            if (context.SubFields.TryGetValue("users", out var usersSubField))
            {
                options.IncludeUsers = true;
                GetUserLookupOptionsForUsers(options, usersSubField);
            }

            if (context.SubFields.TryGetValue("teams", out var teamsSubField))
            {
                options.IncludeTeams = true;
                GetUserLookupOptionsForTeams(options, teamsSubField);
            }

            if (context.SubFields.TryGetValue("owner", out var ownerSubField))
            {
                options.IncludeOwner = true;
            }
        }
        
        private static void GetUserLookupOptionsForUsers(UserLookupOptions options, Field field)
        {
            if (field == null) return;
            options.IncludeFullName = options.IncludeFullName || field.HasSubField("fullName");
            options.IncludeFirstName = options.IncludeFirstName || field.HasSubField("firstName");
            options.IncludeLastName = options.IncludeLastName || field.HasSubField("lastName");
        }
        private static void GetUserLookupOptionsForUsers(UserLookupOptions options, ResolveFieldContext<object> context)
        {
            if (context?.SubFields == null) return;
            options.IncludeFullName = options.IncludeFullName || context.SubFields.ContainsKey("fullName");
            options.IncludeFirstName = options.IncludeFirstName ||  context.SubFields.ContainsKey("firstName");
            options.IncludeLastName = options.IncludeLastName ||  context.SubFields.ContainsKey("lastName");
        }

        private static void GetUserLookupOptionsForTeams(UserLookupOptions options, Field field)
        {
            if (field == null) return;
            var teamMembersSubField = field.GetSubField("users");
            if (teamMembersSubField != null)
            {
                options.IncludeTeamMembers = true;
                GetUserLookupOptionsForUsers(options, teamMembersSubField);
            }
        }
        private static void GetUserLookupOptionsForTeams(UserLookupOptions options, ResolveFieldContext<object> context)
        {
            if (context?.SubFields == null) return;
            if (context.SubFields.TryGetValue("users", out var teamMembersSubField))
            {
                options.IncludeTeamMembers = true;
                GetUserLookupOptionsForUsers(options, teamMembersSubField);
            }
        }
    }
}
