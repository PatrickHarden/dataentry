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
    public class DataEntryMutationUnPublishListingTest
    {
        private readonly FieldType _listingField;

        public DataEntryMutationUnPublishListingTest()
        {
            // Mock service layer
            var publishingService = new Mock<IPublishingService>();
            publishingService.Setup(x => x.UnPublishListingAsync(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .Returns((int id, ClaimsPrincipal user) => Task.FromResult(new ListingViewModel()
                    {
                        Id = id,
                    }
                ));

            // Mock dependency resolver
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(x => x.HttpContext.RequestServices.GetService(typeof(IPublishingService)))
                .Returns(publishingService.Object);

            // Construct
            var mutation = new DataEntryMutation(httpContextAccessor.Object);

            _listingField = mutation.GetField("unpublishListing");
        }

        [Theory]
        [InlineData(1)]
        public async void ListingMutation_HasArguments_ReturnsListingViewModelAsyc(int listingId)
        {
            // Arrange
            var user = new ClaimsPrincipal();

            var context = new Mock<ResolveFieldContext>().Object;
            context.Arguments = new Dictionary<string, object>()
            {
                { "id", listingId }
            };
            context.UserContext = user;

            // Act 
            var result = await (Task<object>)_listingField.Resolver.Resolve(context);

            // Assert
            Assert.IsType<ListingViewModel>(result);
            var listing = (ListingViewModel)result;
            Assert.True(listing.Id == listingId);
        }
    }
}