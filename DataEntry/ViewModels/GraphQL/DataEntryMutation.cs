using dataentry.AutoGraph;
using dataentry.Services.Business.Listings;
using dataentry.Services.Business.Publishing;
using dataentry.Services.Business.Users;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using dataentry.Services.Business.Contacts;
using dataentry.Services.Business.Regions;
using dataentry.Services.Integration.SearchApi;

namespace dataentry.ViewModels.GraphQL
{
    public class DataEntryMutation : ScopedObjectGraphType
    {
        public DataEntryMutation(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            Name = "CreateListingMutation";

            FieldAsync<AutoObjectGraphType<ListingViewModel>>(
                "createListing",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<AutoInputObjectGraphType<ListingViewModel>>> { Name = "listing" }
                ),
                resolve: async context =>
                {
                    var vm = context.GetArgument<ListingViewModel>("listing");
                    var user = (ClaimsPrincipal)context.UserContext;
                    var listingService = base.GetService<IListingService>();
                    return await listingService.CreateListingAsync(user, vm);
                });

            FieldAsync<AutoObjectGraphType<ListingViewModel>>(
                "updateListing",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<AutoInputObjectGraphType<ListingViewModel>>> { Name = "listing" }
                ),
                resolve: async context =>
                {
                    var vm = context.GetArgument<ListingViewModel>("listing");
                    var user = (ClaimsPrincipal)context.UserContext;
                    var listingService = base.GetService<IListingService>();
                    return await listingService.UpdateListingAsync(user, vm);
                });

            FieldAsync<BooleanGraphType>(
                "deleteListing",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" }
                ),
                resolve: async context => {
                    var user = (ClaimsPrincipal)context.UserContext;
                    var id = context.GetArgument<int>("id");
                    var listingService = base.GetService<IListingService>();
                    return await listingService.DeleteListingAsync(user, id);
                });

            FieldAsync<AutoObjectGraphType<ListingViewModel>>(
                "unpublishListing",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" }),
                resolve: async context =>
                {
                    var id = context.GetArgument<int>("id");
                    var user = (ClaimsPrincipal)context.UserContext;
                    var publishingService = base.GetService<IPublishingService>();
                    return await publishingService.UnPublishListingAsync(id, user);
                });

            FieldAsync<AutoObjectGraphType<ListingViewModel>>(
                "publishListing",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" }),
                resolve: async context =>
                {
                    var id = context.GetArgument<int>("id");
                    var user = (ClaimsPrincipal)context.UserContext;
                    var publishingService = base.GetService<IPublishingService>();
                    return await publishingService.PublishListingAsync(id, user);
                });

            FieldAsync<AutoObjectGraphType<ListingViewModel>>(
                "runPublishing",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<AutoInputObjectGraphType<PublishingOptions>>> { Name = "options" }),
                resolve: async context =>
                {
                    var options = context.GetArgument<PublishingOptions>("options");
                    options.UserPrincipal = (ClaimsPrincipal)context.UserContext;
                    var publishingService = base.GetService<IPublishingService>();
                    return await publishingService.RunPublishingAsync(options);
                });

            FieldAsync<BooleanGraphType>(
                "createTeam",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "name" },
                    new QueryArgument<ListGraphType<NonNullGraphType<StringGraphType>>> { Name = "users" }),
                resolve: async context =>
                {
                    var name = context.GetArgument<string>("name");
                    var users = context.GetArgument<IEnumerable<string>>("users");
                    var userService = base.GetService<IUserService>();
                    var result = await userService.CreateTeam(name, users);
                    if (result.Succeeded) return true;
                    context.Errors.AddRange(result.Errors.Select(IdentityErrorToExecutionError));
                    return false;
                });

            FieldAsync<BooleanGraphType>(
                "updateTeam",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "name" },
                    new QueryArgument<StringGraphType> { Name = "newName"},
                    new QueryArgument<ListGraphType<NonNullGraphType<StringGraphType>>> { Name = "users" }),
                resolve: async context =>
                {
                    var principal = (ClaimsPrincipal)context.UserContext;
                    var name = context.GetArgument<string>("name");
                    var newName = context.GetArgument<string>("newName") ?? name;
                    var users = context.GetArgument<IEnumerable<string>>("users");
                    var userService = base.GetService<IUserService>();
                    var result = await userService.UpdateTeam(principal, name, newName, users);
                    if (result.Succeeded) return true;
                    context.Errors.AddRange(result.Errors.Select(IdentityErrorToExecutionError));
                    return false;
                });

            FieldAsync<BooleanGraphType>(
                "deleteTeam",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "name" }),
                resolve: async context =>
                {
                    var principal = (ClaimsPrincipal)context.UserContext;
                    var name = context.GetArgument<string>("name");
                    var userService = base.GetService<IUserService>();
                    var result = await userService.DeleteTeam(principal, name);
                    if (result.Succeeded) return true;
                    context.Errors.AddRange(result.Errors.Select(IdentityErrorToExecutionError));
                    return false;
                });

            FieldAsync<AutoObjectGraphType<ContactsViewModel>>(
                "saveContact",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<AutoInputObjectGraphType<ContactsViewModel>>> { Name = "broker" }),
                resolve: async context =>
                {
                    var broker = context.GetArgument<ContactsViewModel>("broker");
                    var contactService = base.GetService<IContactService>();
                    return await contactService.SaveBroker(broker);
                });

            FieldAsync<AutoObjectGraphType<RegionViewModel>>(
                "createRegion",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<AutoInputObjectGraphType<RegionViewModel>>> { Name = "region" }
                ),
                resolve: async context =>
                {
                    var vm = context.GetArgument<RegionViewModel>("region");
                    var user = (ClaimsPrincipal)context.UserContext;
                    var regionService = base.GetService<IRegionService>();
                    return await regionService.CreateRegionAsync(user, vm);
                });

            FieldAsync<AutoObjectGraphType<RegionViewModel>>(
                "updateRegion",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<AutoInputObjectGraphType<RegionViewModel>>> { Name = "region" }
                ),
                resolve: async context =>
                {
                    var vm = context.GetArgument<RegionViewModel>("region");
                    var user = (ClaimsPrincipal)context.UserContext;
                    var regionService = base.GetService<IRegionService>();
                    return await regionService.UpdateRegionAsync(user, vm);
                });

            FieldAsync<BooleanGraphType>(
                "deleteRegion",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id" }
                ),
                resolve: async context => {
                    var user = (ClaimsPrincipal)context.UserContext;
                    var id = new Guid(context.GetArgument<string>("id"));
                    var regionService = base.GetService<IRegionService>();
                    return await regionService.DeleteRegionAsync(user, id);
                });
            
            FieldAsync<AutoObjectGraphType<ImportListingViewModel>>
            (
                "importPropertiesBySearch",
                description: "Query Search API for properties and import them into Data Entry",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "homeSiteID" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "query" },
                    new QueryArgument<BooleanGraphType> { Name = "overwrite" },
                    new QueryArgument<ListGraphType<StringGraphType>> { Name = "assignToUsers" },
                    new QueryArgument<ListGraphType<StringGraphType>> { Name = "assignToTeams" }
                ),
                resolve: async context =>
                {
                    var user = (ClaimsPrincipal)context.UserContext;
                    var searchApiService = base.GetService<ISearchApiService>();
                    var homeSiteID = context.GetArgument<string>("homeSiteID");
                    var query = context.GetArgument<string>("query");
                    var overwrite = context.GetArgument<bool>("overwrite", false);
                    var assignToUsers = context.GetArgument<List<string>>("assignToUsers", null);
                    var assignToTeams = context.GetArgument<List<string>>("assignToTeams", null);

                    return await searchApiService.ImportListingsByQuery(user, homeSiteID, query, overwrite, assignToUsers, assignToTeams);
                }
            );
        }

        private static ExecutionError IdentityErrorToExecutionError(IdentityError identityError) =>
            new ExecutionError(identityError.Description) { Code = identityError.Code };
    }
}
