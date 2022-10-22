using dataentry.Controllers;
using dataentry.Data.DBContext.Model;
using dataentry.Extensions;
using dataentry.Repository;
using dataentry.Services.Business.BulkUpload;
using dataentry.Services.Business.Configs;
using dataentry.Services.Business.Contacts;
using dataentry.Services.Business.Listings;
using dataentry.ViewModels.GraphQL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using ListingType = dataentry.Data.DBContext.Model.ListingType;

namespace dataentry.Test.Controllers
{
    public class BulkUploadControllerTest : IDisposable
    {
        //private Mock<IConfiguration> _mockConfiguration;
        private BulkUploadService _processor;
        private Mock<ILogger<BulkUploadController>> _mockLogger;
        private Mock<IConfigService> _mockConfigService;
        private BulkUploadController _controller;
        private Mock<IListingService> _mockListingService;

        private List<ListingViewModel> _addedListings;

        public BulkUploadControllerTest()
        {
            //_mockConfiguration = new Mock<IConfiguration>();
            //_mockConfiguration.Setup(configuration => configuration["StoreSettings:ExternalPublishUrl"])
            //    .Returns("http://test.external.publish.url");
            //_mockConfiguration.Setup(configuration => configuration["StoreSettings:ListingOrigin"])
            //    .Returns("TestListingOrigin");

            _processor = new BulkUploadService();

            _addedListings = new List<ListingViewModel>();
            _mockListingService = new Mock<IListingService>();
            _mockListingService.Setup(m => m.CreateListingAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<ListingViewModel>(), false))
                .Returns((ClaimsPrincipal u, ListingViewModel l, bool i) => Task.Run(() =>
                 {
                     _addedListings.Add(l);
                     return l;
                 }));
            _mockLogger = new Mock<ILogger<BulkUploadController>>();

            _mockConfigService = new Mock<IConfigService>();
            _mockConfigService.Setup(m => m.GetDefaultCultureCode()).ReturnsAsync("en-US");
            
            _controller = new BulkUploadController(_processor, _mockListingService.Object, _mockConfigService.Object, _mockLogger.Object);

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        public void Dispose()
        {
        }

        [Fact]
        public async void BulkUploadControllerTest_Success()
        {
            // Arrange
            var fileName = "Global Listings_DataEntry_BulkUpload_Template.xlsx";
            using (var stream = new MemoryStream(GetTestFileData(fileName)))
            {
                var mockFormFile = new Mock<IFormFile>();
                mockFormFile.Setup(m => m.FileName).Returns(fileName);
                mockFormFile.Setup(m => m.OpenReadStream()).Returns(stream);

                // Act
                var response = await _controller.Index(mockFormFile.Object);

                // Assert
                Assert.IsType<OkObjectResult>(response);
                Assert.Throws<ObjectDisposedException>(() => stream.Position);
            }

            Assert.Collection(_addedListings, listing =>
            {
                Assert.Equal("Global Listings_DataEntry_BulkUpload_Template.xlsx", listing.BulkUploadFileName);
                Assert.True(listing.IsBulkUpload);
                Assert.Equal("Record Identifier", listing.PropertyRecordName);
                Assert.Equal("office", listing.PropertyType);
                Assert.Equal("lease", listing.ListingType);
                Assert.Equal("CapitaSpring", listing.PropertyName);
                Assert.Equal("88 Market Street", listing.Street);
                Assert.Null(listing.Street2);
                Assert.Equal("Raffles Place/ Boat Quay", listing.City);
                Assert.Null(listing.StateOrProvince);
                Assert.Equal("70719", listing.PostalCode);
                Assert.Equal("en-US", listing.Headline.Single().CultureCode);
                Assert.Equal("Test Headline", listing.Headline.Single().Text);
                Assert.Equal("en-US", listing.BuildingDescription.Single().CultureCode);
                Assert.Equal("Test Property Description", listing.BuildingDescription.Single().Text);
                Assert.Equal("en-US", listing.LocationDescription.Single().CultureCode);
                Assert.Equal("Using Toggle Street View", listing.LocationDescription.Single().Text);
                Assert.Equal("http://cbrepropertysearch.com.sg/index.php/office/capitaspring-market-street", listing.Website);

                Assert.NotNull(listing.Specifications);
                Assert.Equal("monthly", listing.Specifications.LeaseTerm);
                Assert.Equal("LeaseHold", listing.Specifications.LeaseType);
                Assert.Equal("sf", listing.Specifications.Measure);
                Assert.Equal(10, listing.Specifications.MinSpace);
                Assert.Equal(20, listing.Specifications.TotalSpace);
                Assert.Equal(30, listing.Specifications.MinPrice);
                Assert.Equal(40, listing.Specifications.MaxPrice);
                Assert.False(listing.Specifications.ContactBrokerForPrice);

                //Assert.NotNull(listing.Highlights);
                //Assert.Collection(listing.Highlights.OrderBy(highlight => highlight?.Order),
                //    highlight => Assert.Equal("Great", highlight.Value),
                //    highlight => Assert.Equal("Awesome", highlight.Value)
                //);

                //Assert.NotNull(listing.MicroMarkets);
                //Assert.Collection(listing.MicroMarkets.OrderBy(microMarket => microMarket?.Order),
                //    microMarket => Assert.Equal("CBD: Chinatown/Telok Ayer", microMarket.Value));

                Assert.NotNull(listing.Spaces);
                Assert.Collection(listing.Spaces,
                    space =>
                    {
                        Assert.Equal("en-US", space.Name.Single().CultureCode);
                        Assert.Equal("#01", space.Name.Single().Text);
                        Assert.Equal("available", space.Status);
                        Assert.Equal("industrial", space.SpaceType);
                        Assert.Equal(new DateTime(2019, 8, 20), space.AvailableFrom);
                        Assert.NotNull(space.Specifications);
                        Assert.Equal("sf", space.Specifications.Measure);
                        Assert.Equal(1000, space.Specifications.TotalSpace);
                        Assert.Equal(10, space.Specifications.MaxPrice);
                        Assert.True(space.Specifications.ContactBrokerForPrice);
                    },
                    space =>
                    {
                        Assert.Equal("en-US", space.Name.Single().CultureCode);
                        Assert.Equal("#02", space.Name.Single().Text);
                        Assert.Equal("unavailable", space.Status);
                        Assert.Equal("office", space.SpaceType);
                        Assert.Equal(new DateTime(2019, 8, 21), space.AvailableFrom);
                        Assert.NotNull(space.Specifications);
                        Assert.Equal("acre", space.Specifications.Measure);
                        Assert.Equal(2000, space.Specifications.TotalSpace);
                        Assert.Equal(20, space.Specifications.MaxPrice);
                        Assert.False(space.Specifications.ContactBrokerForPrice);
                    });

                Assert.Collection(listing.Contacts,
                    contact =>
                    {
                        Assert.Equal("John", contact.FirstName);
                        Assert.Equal("Smith", contact.LastName);
                        Assert.Equal("john.smith@cbre.com", contact.Email);
                        Assert.Equal("5555555555", contact.Phone);
                        Assert.Equal("Texas - Dallas McKinney", contact.Location);
                        Assert.NotNull(contact.AdditionalFields);
                        Assert.Equal("12345", contact.AdditionalFields.License);
                    },
                    contact =>
                    {
                        Assert.Equal("Jane", contact.FirstName);
                        Assert.Equal("Doe", contact.LastName);
                        Assert.Equal("jane.doe@cbre.com", contact.Email);
                        Assert.Equal("5555555556", contact.Phone);
                        Assert.Equal("Texas - Dallas Park Lane", contact.Location);
                        Assert.NotNull(contact.AdditionalFields);
                        Assert.Equal("23456", contact.AdditionalFields.License);
                    });
            });
        }


        [Fact]
        public async void BulkUploadControllerTest_Success_FlexSpaces()
        {
            // Arrange
            var fileName = "Global Listings_DataEntry_BulkUpload_Template_FlexSpaces.xlsx";
            using (var stream = new MemoryStream(GetTestFileData(fileName)))
            {
                var mockFormFile = new Mock<IFormFile>();
                mockFormFile.Setup(m => m.FileName).Returns(fileName);
                mockFormFile.Setup(m => m.OpenReadStream()).Returns(stream);

                // Act
                var response = await _controller.Index(mockFormFile.Object);

                // Assert
                Assert.IsType<OkObjectResult>(response);
                Assert.Throws<ObjectDisposedException>(() => stream.Position);
            }

            Assert.Collection(_addedListings, listing =>
            {
                Assert.Collection(listing.Spaces,
                    space =>
                    {
                        Assert.Equal("en-US", space.Name.Single().CultureCode);
                        Assert.Equal("#01", space.Name.Single().Text);
                        Assert.Equal("available", space.Status);
                        Assert.Equal(new DateTime(2019, 8, 20), space.AvailableFrom);
                        Assert.NotNull(space.Specifications);
                        Assert.Equal("sf", space.Specifications.Measure);
                        Assert.Equal(11, space.Specifications.MinSpace);
                        Assert.Equal(21, space.Specifications.MaxSpace);
                        Assert.Equal(10, space.Specifications.MinPrice);
                        Assert.True(space.Specifications.ContactBrokerForPrice);
                    },
                    space =>
                    {
                        Assert.Equal("en-US", space.Name.Single().CultureCode);
                        Assert.Equal("#02", space.Name.Single().Text);
                        Assert.Equal("unavailable", space.Status);
                        Assert.Equal(new DateTime(2019, 8, 21), space.AvailableFrom);
                        Assert.NotNull(space.Specifications);
                        Assert.Equal("acre", space.Specifications.Measure);
                        Assert.Equal(12, space.Specifications.MinSpace);
                        Assert.Equal(22, space.Specifications.MaxSpace);
                        Assert.Equal(20, space.Specifications.MinPrice);
                        Assert.False(space.Specifications.ContactBrokerForPrice);
                    });
            });
        }


        [Fact]
        public async void BulkUploadControllerTest_InvalidData()
        {
            // Arrange
            var fileName = "BulkUploadControllerTest_Invalid.xlsx";
            using (var stream = new MemoryStream(GetTestFileData(fileName)))
            {
                var mockFormFile = new Mock<IFormFile>();
                mockFormFile.Setup(m => m.FileName).Returns(fileName);
                mockFormFile.Setup(m => m.OpenReadStream()).Returns(stream);

                // Act
                var response = await _controller.Index(mockFormFile.Object);

                // Assert
                // TODO: use a non-anonymous type as the response value to allow testing values
            }
        }

        [Fact]
        public async void BulkUploadControllerTest_BadExtension()
        {

            // Arrange
            var fileName = "EvilFile.exe";
            var mockFormFile = new Mock<IFormFile>();
            mockFormFile.Setup(m => m.FileName).Returns(fileName);
            mockFormFile.Setup(m => m.OpenReadStream()).Throws(new InvalidDataException());

            try
            {
                // Act
                var response = await _controller.Index(mockFormFile.Object);
            }
            catch (InvalidDataException)
            {
                Assert.False(true, "BulkUploadController should not try to access the stream.");
            }
        }

        private byte[] GetTestFileData(string fileName)
        {
            // Using byte array instead of file stream on purpose, don't want to leave it open during the test
            return File.ReadAllBytes(Path.Combine("Controllers", "BulkUploadControllerTest_Data", fileName));
        }
    }
}
