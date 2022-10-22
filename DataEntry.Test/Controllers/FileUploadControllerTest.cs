using System;
using System.IO;
using System.Threading.Tasks;
using dataentry.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using dataentry.Services.Business.Images;

namespace dataentry.Controllers.Test
{
    public class FileUploadControllerTest
    {
        private FileUploadController FileUploadController;
        private Mock<ILogger<FileUploadController>> _mockLogger;
        private Mock<System.Net.Http.IHttpClientFactory> _mockHttp;
        private Mock<IImageService> _mockImageService;
        public FileUploadControllerTest()
        {
            var MockAzureStorageRepository = new Mock<IAzureStorageRepository>();
            var config = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.test.json", false, true)
                    .Build();
            
            MockAzureStorageRepository.Setup(_ => _.UploadBlobAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>())).Returns(Task.FromResult("Test"));
            MockAzureStorageRepository.Setup(_ => _.ExistsAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(true));
            _mockLogger = new Mock<ILogger<FileUploadController>>();
            _mockHttp = new Mock<System.Net.Http.IHttpClientFactory>();
            _mockImageService = new Mock<IImageService>();

            FileUploadController = new FileUploadController(MockAzureStorageRepository.Object, config, _mockLogger.Object, _mockHttp.Object, _mockImageService.Object);
        }

        [Theory]
        [InlineData("image/jpeg", 1024, 200)]
        public async Task FileUploadController_UploadValidFile_ReturnsSuccess(string type, int length, int expected)
        {
            // Arrange.
            var file = GenerateMockFile(type, length);

            //Act
            var result = await FileUploadController.Image(file);

            //Assert
            var objectResponse = result as ObjectResult;
            Assert.Equal(expected, objectResponse.StatusCode);
        }

        [Theory]
        [InlineData("application/octet-stream", 1024, 400)]
        public async Task FileUploadController_UploadInValidFile_ReturnsFail(string type, int length, int expected)
        {
            // Arrange.
            var file = GenerateMockFile(type, length);

            //Act
            var result = await FileUploadController.Image(file);

            //Assert
            var objectResponse = result as ObjectResult;
            Assert.Equal(expected, objectResponse.StatusCode);
        }

        [Theory]
        [InlineData("application/octet-stream", 1024, 400, "Invalid image file format")]
        [InlineData("image/jpeg", 0, 400, "Upload image with file size greater than zero")]
        public async Task FileUploadController_UploadInvalidFormat_ThrowsErrorMessage(string type, int length, int expected, string expectedMessage)
        {
            // Arrange.
            var file = GenerateMockFile(type, length);

            //Act
            var result = await FileUploadController.Image(file);

            //Assert
            var objectResponse = result as ObjectResult;
            Assert.Equal(expected, objectResponse.StatusCode);
            var badresult = result as BadRequestObjectResult;
            Assert.Equal(expectedMessage, badresult.Value);
        }

        private IFormFile GenerateMockFile(string type, int length)
        {
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(_ => _.ContentType).Returns(type);
            fileMock.Setup(_ => _.FileName).Returns("Test");
            fileMock.Setup(_ => _.Length).Returns(length);
            var file = fileMock.Object;
            return file;
        }
    }
}