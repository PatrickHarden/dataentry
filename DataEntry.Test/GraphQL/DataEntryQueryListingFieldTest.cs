using dataentry.Services.Business.Listings;
using dataentry.Services.Business.Users;
using dataentry.ViewModels.GraphQL;
using Microsoft.AspNetCore.Http;
using GraphQL.Types;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace dataentry.Test.GraphQL
{
    public class DataEntryQueryListingFieldTest
    {
        private readonly FieldType _listingField;

        public DataEntryQueryListingFieldTest()
        {
            // Mock service layer
            var listingService = new Mock<IListingService>();
            listingService.Setup(x => x.GetListingById(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>(), It.IsAny<UserLookupOptions>()))
                .Returns((ClaimsPrincipal user, int id, UserLookupOptions options) => Task.FromResult(new ListingViewModel()
                {
                    Id = id
                }));

            // Mock dependency resolver
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(x => x.HttpContext.RequestServices.GetService(typeof(IListingService)))
                .Returns(listingService.Object);

            // Construct
            var query = new DataEntryQuery(httpContextAccessor.Object);

            _listingField = query.GetField("listing");
        }

        [Theory]
        [InlineData(1)]
        public async void ListingQuery_HasArguments_ReturnsListingAsync(int listingId)
        {
            // Arrange
            var context = new Mock<ResolveFieldContext>().Object;

            context.UserContext = new ClaimsPrincipal();
            context.Arguments = new Dictionary<string, object>()
            {
                { "id", listingId }
            };

            // Act
            var result = await (Task<object>)_listingField.Resolver.Resolve(context);

            // Assert
            Assert.IsType<ListingViewModel>(result);
            var listing = (ListingViewModel)result;
            Assert.True(listing.Id == listingId);
        }
    }
}
