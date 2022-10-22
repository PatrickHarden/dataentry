using dataentry.Services.Business.Listings;
using dataentry.ViewModels.GraphQL;
using Microsoft.AspNetCore.Http;
using GraphQL.Types;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace dataentry.Test.GraphQL
{
    public class DataEntryMutationCreateListingTest
    {
        private readonly FieldType _listingField;
        public DataEntryMutationCreateListingTest()
        {
            // Mock service layer
            var listingService = new Mock<IListingService>();
            listingService.Setup(x => x.CreateListingAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<ListingViewModel>(), false))
                .Returns((ClaimsPrincipal user, ListingViewModel vm, bool imported) => Task.FromResult(
                    new ListingViewModel
                    {
                        Id = 1,
                        PropertyName = vm.PropertyName
                    }
                ));

            // Mock dependency resolver
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(x => x.HttpContext.RequestServices.GetService(typeof(IListingService)))
                .Returns(listingService.Object);

            // Construct
            var mutation = new DataEntryMutation(httpContextAccessor.Object);

            _listingField = mutation.GetField("createListing");
        }

        [Theory]
        [ClassData(typeof(ListingViewModelData))]
        public async void ListingMutation_HasArguments_ReturnsListingViewModelAsync(ListingViewModel vm)
        {
            // Arrange
            var user = new ClaimsPrincipal();

            var context = new Mock<ResolveFieldContext>().Object;
            context.Arguments = new Dictionary<string, object>()
            {
                { "listing", vm }
            };
            context.UserContext = user;

            // Act 
            dynamic result = await (Task<object>)_listingField.Resolver.Resolve(context);

            // Assert
            Assert.IsType<ListingViewModel>(result);
            var listing = (ListingViewModel)result;
            Assert.Equal(listing.PropertyName, vm.PropertyName);
        }
    }

    public class ListingViewModelData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                new ListingViewModel
                {
                  PropertyName = "test"
                }
            };
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
