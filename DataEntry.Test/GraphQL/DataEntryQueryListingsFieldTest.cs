using dataentry.Services.Business.Listings;
using dataentry.Services.Business.Users;
using dataentry.ViewModels.GraphQL;
using Microsoft.AspNetCore.Http;
using GraphQL.Types;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using dataentry.Data.DBContext.Model;

namespace dataentry.Test.GraphQL
{
    public class DataEntryQueryListingsFieldTest
    {
        private readonly FieldType _listingField;

        public DataEntryQueryListingsFieldTest()
        {

            // Mock service layer
            var listingService = new Mock<IListingService>();
            listingService.Setup(x => x.GetListings(It.IsAny<ClaimsPrincipal>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<UserLookupOptions>(), It.IsAny<IEnumerable<FilterViewModel>>(), It.IsAny<string>()))
                .Returns((ClaimsPrincipal user, int? skip, int? take, UserLookupOptions options, IEnumerable<FilterViewModel> filters, string regionID) =>
                {
                    return Task.FromResult(new List<ListingViewModel>()
                    {
                        new ListingViewModel() { Id = 0 },
                        new ListingViewModel() { Id = 1 },
                    }.AsEnumerable());
                });

            // Mock dependency resolver
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(x => x.HttpContext.RequestServices.GetService(typeof(IListingService)))
                .Returns(listingService.Object);

            // Construct
            var query = new DataEntryQuery(httpContextAccessor.Object);

            _listingField = query.GetField("listings");
        }

        [Fact]
        public async void ListingsQuery_HasArguments_ReturnsListingsAsync()
        {
            // Arrange
            var context = new Mock<ResolveFieldContext>().Object;

            context.UserContext = new ClaimsPrincipal();

            // Act
            var listings = await (Task<object>)_listingField.Resolver.Resolve(context) as IEnumerable<ListingViewModel>;

            // Assert
            Assert.True(listings.Any());
        }
    }
}
