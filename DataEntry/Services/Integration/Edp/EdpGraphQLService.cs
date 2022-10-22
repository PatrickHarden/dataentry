using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using dataentry.Services.Integration.Edp.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace dataentry.Services.Integration.Edp
{
    public class EdpGraphQLService : IEdpGraphQLService
    {
        private readonly HttpClient _httpClient;
        private readonly EdpOptions _edpOptions;
        private readonly ILogger<EdpGraphQLService> _log;

        public EdpGraphQLService(HttpClient httpClient, IOptions<EdpOptions> edpOptions, ILogger<EdpGraphQLService> log)
        {
            _httpClient = httpClient ??
                throw new ArgumentNullException(nameof(httpClient));
            _edpOptions = edpOptions?.Value ??
                throw new ArgumentNullException(nameof(edpOptions));
            _log = log ??
                throw new ArgumentNullException(nameof(log));

            if (_edpOptions.Enabled)
            {
                if (_edpOptions.Endpoint == null) LogNullConfigWarning("Edp:Endpoint");
                if (_edpOptions.SourceSystemName == null) LogNullConfigWarning("Edp:SourceSystemName");
                if (_edpOptions.SourceSubmitterName == null) LogNullConfigWarning("Edp:SourceSubmitterName");
                if (_edpOptions.UserRole == null) LogNullConfigWarning("Edp:UserRole");
            }
        }

        private void LogNullConfigWarning(string configKey)
        {
            _log.LogWarning("Config value is null: {configKey}", configKey);
        }

        public async Task<string> GetApiVersionAsync()
        {
            return await RunQuery("{getAPIVersion}");
        }

        public async Task<string> RunQueryRaw<T>(EdpGraphQLQuery<T> query) where T : EdpGraphQLObject, new()
        {
            string requestId = ((dynamic)query)?.request?.request_id;
            using(_log.BeginScope(new Dictionary<string, object>
            {
                ["EdpGraphQLService_requestId"] = requestId
            }))
            {
                return await RunQuery(query.ToString());
            }
        }

        public Task<T> RunQuery<T>(EdpGraphQLQuery<T> query) where T : EdpGraphQLObject, new()
        {
            //TODO: Currently EDP's schema says the result data is string but they actually return an object, which breaks stuff, use RunQueryRaw for now.
            throw new NotImplementedException();

            // var resultString = await RunQueryRaw(query);
            // return JsonConvert.DeserializeObject<T>(resultString);
        }

        public async Task<string> RunQuery(string query)
        {
            _log.LogInformation("Sending GraphQL query to EDP: {edpQuery}", query);
            var encodedQuery = HttpUtility.JavaScriptStringEncode(query);
            var queryEnvelope = $"{{\"query\":\"{encodedQuery}\"}}";
            var requestContent = new StringContent(queryEnvelope, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_edpOptions.Endpoint, requestContent);
            if (!response.IsSuccessStatusCode)
            {
                _log.LogError("EDP returned a non-successful response: {0}: {1}", response.StatusCode, response.ReasonPhrase);
            }
            else
            {
                _log.LogInformation("EDP returned a successful response: {0}: {1}", response.StatusCode, response.ReasonPhrase);
            }
            var responseContent = await response.Content.ReadAsStringAsync();
            _log.LogInformation("Response from EDP with code ({edpResponseCode}): {edpResponse}", response.StatusCode, responseContent);

            return responseContent;
        }
    }
}