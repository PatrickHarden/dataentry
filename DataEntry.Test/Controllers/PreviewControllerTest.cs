using dataentry.Controllers;
using dataentry.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using System.IO;
using Xunit;

namespace dataentry.Test.Controllers
{
    public class PreviewControllerTest
    {
        private IOptions<Configs> _config;

        public PreviewControllerTest()
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", false)
               .AddJsonFile("appsettings.Local.json", false)
               .Build();

            _config = Options.Create(configuration.Get<Configs>());
        }

        [Theory]
        [InlineData("us-comm", "office")]
        [InlineData("us-comm", "officecoworking")]
        [InlineData("us-comm", "industrial")]
        [InlineData("sg-comm", "office")]
        [InlineData("sg-comm", "retail")]
        [InlineData("sg-comm", "industrial")]
        [InlineData("in-comm", "office")]
        [InlineData("in-comm", "retail")]
        [InlineData("in-comm", "flexindustrial")]
        public void PreviewController_GetSPAValidUrl_ReturnsOk(string homeSiteId, string usageType)
        {
            // Arrange
            var previewController = new PreviewController(_config);

            // Act
            var response = previewController.Index(homeSiteId, usageType);

            // Assert
            Assert.IsType<ViewResult>(response);
        }

        [Theory]
        [InlineData("", "office")]
        [InlineData("null", "retail")]
        [InlineData(null, "industrial")]
        public void PreviewController_GetSPAInvalidUrl_ReturnsNotfound(string homeSiteId, string usageType)
        {
            // Arrange
            var previewController = new PreviewController(_config);

            // Act
            var response = previewController.Index(homeSiteId, usageType);

            // Assert
            Assert.IsType<NotFoundObjectResult>(response);
        }
    }
}
