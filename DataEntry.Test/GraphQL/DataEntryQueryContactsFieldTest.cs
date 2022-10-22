using dataentry.Services.Business.Contacts;
using dataentry.ViewModels.GraphQL;
using Microsoft.AspNetCore.Http;
using GraphQL.Types;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace dataentry.Test.GraphQL
{
    public class DataEntryQueryContactsFieldTest
    {
        private readonly FieldType _listingField;

        public DataEntryQueryContactsFieldTest()
        {
            // Mock service layer
            var contactService = new Mock<IContactService>();
            contactService.Setup(x => x.GetAllBrokers())
                .Returns(() => {
                    return Task.FromResult(new List<ContactsViewModel>()
                    {
                        new ContactsViewModel() { ContactId = 0 },
                        new ContactsViewModel() { ContactId = 1 },
                    }.AsEnumerable());
                });

            // Mock dependency resolver
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(x => x.HttpContext.RequestServices.GetService(typeof(IContactService)))
                .Returns(contactService.Object);

            // Construct
            var query = new DataEntryQuery(httpContextAccessor.Object);

            _listingField = query.GetField("contacts");
        }

        [Fact]
        public async void ContactsQuery_HasNoArguments_ReturnsBrokersAsync()
        {
            // Arrange
            var context = new Mock<ResolveFieldContext>().Object;

            // Act
            var brokers = await (Task<object>)_listingField.Resolver.Resolve(context) as IEnumerable<ContactsViewModel>;

            // Assert
            Assert.True(brokers.Any());
        }
    }
}
