using dataentry.Services.Business.Listings;
using dataentry.Services.Business.Publishing;
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
    public class DataEntryMutationUpdateListingFieldTest
    {
        private readonly FieldType _listingField;

        public DataEntryMutationUpdateListingFieldTest()
        {

            // Mock service layer
            var listingService = new Mock<IListingService>();
            listingService.Setup(x => x.UpdateListingAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<ListingViewModel>()))
                .Returns((ClaimsPrincipal user, ListingViewModel vm) => Task.FromResult(vm));

            // Mock dependency resolver
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(x => x.HttpContext.RequestServices.GetService(typeof(IListingService)))
                .Returns(listingService.Object);

            // Construct
            var mutation = new DataEntryMutation(httpContextAccessor.Object);

            _listingField = mutation.GetField("updateListing");
        }

        [Theory]
        [InlineData(1)]
        public async void UpdateListingMutation_HasArguments_ReturnsListingAsync(int listingId)
        {
            // Arrange
            var context = new Mock<ResolveFieldContext>().Object;

            context.UserContext = new ClaimsPrincipal();
            context.Arguments = new Dictionary<string, object>()
            {
                { "listing", new ListingViewModel() { Id = listingId } }
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
