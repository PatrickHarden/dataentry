using dataentry.ViewModels.GraphQL;
using GraphQL;
using GraphQL.Types;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dataentry.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GraphQLController : ControllerBase
    {
        private readonly IDocumentExecuter _documentExecuter;
        private readonly ISchema _schema;
        private readonly ILogger<GraphQLController> _logger;

        public GraphQLController(ISchema schema, IDocumentExecuter documentExecuter, ILogger<GraphQLController> logger)
        {
            _documentExecuter = documentExecuter;
            _schema = schema;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQLQuery query)
        {
            if (query == null) { throw new ArgumentNullException(nameof(query)); }
            var inputs = query.Variables.ToInputs();
            var executionOptions = new ExecutionOptions
            {
                Schema = _schema,
                Query = query.Query,
                Inputs = inputs,
                UserContext = User
            };

            ExecutionResult result;
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["GraphQLController_userName"] = User.Identity.Name
            }))
            {
                result = await _documentExecuter.ExecuteAsync(executionOptions).ConfigureAwait(false);
            }

            if (result.Errors?.Count > 0)
            {
                var telemetry = HttpContext.Features.Get<RequestTelemetry>();
                if (telemetry != null)
                {
                    telemetry.Properties.Add("Body", query.Query);
                    telemetry.Properties.Add("User Name", User.Identity.Name);
                }
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}