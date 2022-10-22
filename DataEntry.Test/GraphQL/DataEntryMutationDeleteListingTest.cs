using dataentry.Services.Business.Listings;
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
    public class DataEntryMutationDeleteListingTest
    {
        private readonly FieldType _listingField;
        public DataEntryMutationDeleteListingTest()
        {
            // Mock service layer
            var listingService = new Mock<IListingService>();
            listingService.Setup(x => x.DeleteListingAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
              .Returns((ClaimsPrincipal user, int id) => Task.FromResult(true));

            // Mock dependency resolver
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(x => x.HttpContext.RequestServices.GetService(typeof(IListingService)))
                .Returns(listingService.Object);

            // Construct
            var mutation = new DataEntryMutation(httpContextAccessor.Object);

            _listingField = mutation.GetField("deleteListing");
        }

        [Theory]
        [InlineData(1)]
        public async void ListingMutation_HasArguments_ReturnSuccessAsyc(int listingId)
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
            Assert.IsType<bool>(result);
            var resultBool = (bool)result;
            Assert.True(resultBool);
        }
    }
}