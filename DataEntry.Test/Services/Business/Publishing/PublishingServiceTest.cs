using dataentry.Data.DBContext.Model;
using dataentry.Extensions;
using dataentry.Repository;
using dataentry.Services.Business.Listings;
using dataentry.Services.Business.Publishing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;

namespace dataentry.Test.Services.Business.Publishing
{
    public class PublishingServiceTest
    {
        private const int _testId = 1234;
        private PublishingService _publishingService;
        private Listing _testListing;
        private Mock<IDataEntryRepository> _mockDataEntryRepository;
        private Mock<IListingMapper> _mockListingMapper;
        private Mock<ILogger<PublishingService>> _mockLogger;
        private Mock<IPublishingTarget> _mockPublishingTarget1;
        private Mock<IPublishingTarget> _mockPublishingTarget2;
        private ClaimsPrincipal _user;

        public PublishingServiceTest()
        {
            _mockDataEntryRepository = new Mock<IDataEntryRepository>();
            _mockListingMapper = new Mock<IListingMapper>();
            _mockLogger = new Mock<ILogger<PublishingService>>();
            _mockPublishingTarget1 = new Mock<IPublishingTarget>();
            _mockPublishingTarget2 = new Mock<IPublishingTarget>();
            _user = new ClaimsPrincipal();
            var config = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.test.json", false, true)
                    .Build();

            _mockPublishingTarget1.Setup(t => t.Name).Returns("Target1");
            _mockPublishingTarget2.Setup(t => t.Name).Returns("Target2");
            var publishingTargets = new List<IPublishingTarget>()
            {
                _mockPublishingTarget1.Object,
                _mockPublishingTarget2.Object
            };
            _publishingService = new PublishingService(_mockDataEntryRepository.Object, _mockListingMapper.Object, publishingTargets, _mockLogger.Object, config);

            _testListing = new Listing();

            _testListing.Region = new Region
            {
                Name = "Test Region",
                CountryCode = "SG",
                CultureCode = "en-SG",
                HomeSiteID = "sg-comm",
                ListingPrefix = "TestListingOrigin",
                PreviewSiteID = "TestPreviewHomeSiteId",
                PreviewPrefix = "TestListingPreviewOrigin",
                ExternalPublishUrl = "TestExternalPublishUrl",
                ExternalPreviewUrl = "TestExternalPreviewUrl"
            };

            _testListing.VerifyExternalID().Wait();

            _mockDataEntryRepository.Setup(r => r.GetListingByID(_testId, _user)).ReturnsAsync(_testListing);
        }

        [Fact]
        public async void PublishingServiceTest_Publish()
        {
            // Arrange
            _mockDataEntryRepository.Setup(r => r.UpdateListing(_testListing, _user, It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>())).ReturnsAsync(() =>
            {
                Assert.Equal("Publishing", _testListing.GetListingData<PublishingState>().Value);
                return _testListing;
            }).Verifiable();

            // Act
            await _publishingService.PublishListingAsync(_testId, _user);

            // Assert
            _mockPublishingTarget1.Verify(t => t.Publish(_testListing), $"{nameof(_mockPublishingTarget1)} was not published");
            _mockPublishingTarget2.Verify(t => t.Publish(_testListing), $"{nameof(_mockPublishingTarget2)} was not published");
            _mockDataEntryRepository.Verify(r => r.UpdateListing(_testListing, _user, It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>()), "Listing was not updated");
        }
        
        [Fact]
        public async void PublishingServiceTest_PartialPublish()
        {
            // Arrange
            _mockDataEntryRepository.Setup(r => r.UpdateListing(_testListing, _user, It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>())).ReturnsAsync(() =>
            {
                Assert.Equal("Publishing", _testListing.GetListingData<PublishingState>().Value);
                return _testListing;
            }).Verifiable();

            var publishingOptions = new PublishingOptions(_testId, _user);
            publishingOptions.PublishingTargets = new List<string>{"target2"};

            // Act
            await _publishingService.RunPublishingAsync(publishingOptions);

            // Assert
            _mockPublishingTarget1.Verify(t => t.Publish(_testListing), Times.Never, $"{nameof(_mockPublishingTarget1)} was published, but was not supposed to be");
            _mockPublishingTarget2.Verify(t => t.Publish(_testListing), $"{nameof(_mockPublishingTarget2)} was not published");
            _mockDataEntryRepository.Verify(r => r.UpdateListing(_testListing, _user, It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>()), "Listing was not updated");
        }

        [Fact]
        public async void PublishingServiceTest_Unpublish()
        {
            // Arrange
            _mockDataEntryRepository.Setup(r => r.UpdateListing(_testListing, _user, It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>())).ReturnsAsync(() =>
            {
                Assert.Equal("Unpublishing", _testListing.GetListingData<PublishingState>().Value);
                return _testListing;
            });

            // Act
            await _publishingService.UnPublishListingAsync(_testId, _user);

            // Assert
            _mockPublishingTarget1.Verify(t => t.Unpublish(_testListing), $"{nameof(_mockPublishingTarget1)} was not unpublished");
            _mockPublishingTarget2.Verify(t => t.Unpublish(_testListing), $"{nameof(_mockPublishingTarget2)} was not unpublished");
            _mockDataEntryRepository.Verify(r => r.UpdateListing(_testListing, _user, It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>()), "Listing was not updated");
        }

        [Fact]
        public async void PublishingServiceTest_PublishPreview()
        {
            // Arrange

            // Act
            await _publishingService.PreviewListingAsync(_testId, _user);

            // Assert
            _mockPublishingTarget1.Verify(t => t.PublishPreview(_testListing), $"{nameof(_mockPublishingTarget1)} preview was not published");
            _mockPublishingTarget2.Verify(t => t.PublishPreview(_testListing), $"{nameof(_mockPublishingTarget2)} preview was not published");
        }

        [Fact]
        public async void PublishingServiceTest_UnpublishPreview()
        {
            // Arrange

            // Act
            await _publishingService.UnPreviewListingAsync(_testId, _user);

            // Assert
            _mockPublishingTarget1.Verify(t => t.UnpublishPreview(_testListing), $"{nameof(_mockPublishingTarget1)} preview was not unpublished");
            _mockPublishingTarget2.Verify(t => t.UnpublishPreview(_testListing), $"{nameof(_mockPublishingTarget2)} preview was not unpublished");
        }

        [Fact]
        public async void PublishingServiceTest_Exceptions()
        {
            // Arrange
            var exception1 = new Exception();
            var exception2 = new Exception();
            _mockPublishingTarget1.Setup(t => t.Publish(_testListing)).ThrowsAsync(exception1);
            _mockPublishingTarget2.Setup(t => t.Publish(_testListing)).ThrowsAsync(exception2);
            _mockPublishingTarget1.Setup(t => t.StopOnException).Returns(true);

            // Act
            await Assert.ThrowsAsync<PublishingException>(() => _publishingService.PublishListingAsync(_testId, _user));

            // Assert
            _mockLogger.Verify(l => l.Log
                (
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsNotNull<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)
                ), 
                Times.Exactly(2), "Did not detect two error messages in Logger"
            );
        }

        [Theory]
        [InlineData("Published")]
        [InlineData("PublishFailed")]
        [InlineData("Unpublished")]
        [InlineData("UnpublishFailed")]
        public async void PublishingServiceTest_ValidStates(string invalidState)
        {
            // Arrange
            _testListing.SetListingData(new PublishingState { Value = invalidState });

            // Act
            await _publishingService.PublishListingAsync(_testId, _user);
        }

        [Theory]
        [InlineData("Publishing")]
        [InlineData("Unpublishing")]
        public async void PublishingServiceTest_InvalidStates(string invalidState)
        {
            // Arrange
            _testListing.SetListingData(new PublishingState { Value = invalidState });

            // Act
            await Assert.ThrowsAsync<InvalidOperationException>(() => _publishingService.PublishListingAsync(_testId, _user));
        }
    }
}
