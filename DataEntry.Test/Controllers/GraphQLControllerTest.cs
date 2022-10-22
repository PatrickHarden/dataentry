using dataentry.Controllers;
using dataentry.ViewModels.GraphQL;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace dataentry.Test.Controllers
{
    public class GraphQLControllerTest
    {
        [Fact]
        public void GraphQLController_PostValidRequest_ReturnsOk()
        {
            // Arrange
            var schema = new Mock<ISchema>();
            var documentExecuter = new Mock<IDocumentExecuter>();
            var logger = new Mock<ILogger<GraphQLController>>();
            var controllerContext = new ControllerContext();
            var httpContext = new Mock<HttpContext>();

            documentExecuter.Setup(de =>
                de.ExecuteAsync(It.IsAny<ExecutionOptions>())
            ).Returns(() => {
                return Task.FromResult(new ExecutionResult());
            });

            httpContext.Setup(c => c.User.Identity.Name).Returns("TestUser");
            controllerContext.HttpContext = httpContext.Object;

            var graphQLController = new GraphQLController(schema.Object, documentExecuter.Object, logger.Object);
            graphQLController.ControllerContext = controllerContext;
            var graphQLQuery = new GraphQLQuery()
            {
                Query = "{\n  listing {\n    id\n  }\n}"
            };

            // Act
            var response = graphQLController.Post(graphQLQuery).Result;

            // Assert
            Assert.IsType<OkObjectResult>(response);
        }
    }
}
