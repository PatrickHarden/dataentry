using dataentry.Services.Business.Publishing;
using dataentry.Services.Integration.StoreApi.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace dataentry.Services.Integration.StoreApi
{
    public class StoreService : IStoreService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public StoreService(HttpClient httpClient, ILogger<StoreService> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private string GetDeleteBody(string primaryKey)
        {
            DeleteBody deleteBody = new DeleteBody()
            {
                PropertyListing = new PropertyListing()
                {
                    CommonPrimaryKey = primaryKey,
                    CommonSource = "default",
                    CommonEntityType = "PropertyListing"
                }
            };

            // default value handling ignore prevents uninitialized integers from being serialized
            var _deleteBody = JsonConvert.SerializeObject(deleteBody, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore });

            return _deleteBody;
        }

        /// <summary>
        /// Used to make http post request to StoreAPI to add listing
        /// </summary>
        /// <param name="propertyListing"></param>
        /// <returns></returns>
        public async Task<dynamic> AddListing(PropertyListing propertyListing)
        {
            // Setup the url for the store api
            var source = propertyListing.CommonPrimaryKey.StartsWith("NL-") ? "DP" : "default";
            var request = new HttpRequestMessage(HttpMethod.Post, $"api/PropertyListing/Post/{propertyListing.CommonPrimaryKey}?source={source}");

            // Setup headers for the http message
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // The store api expects to have the the property listing as a child in the request
            PropertyListingEnvelope propertyListingEnvelope = new PropertyListingEnvelope();
            propertyListingEnvelope.PropertyListing = propertyListing;

            // Set content for the http message
            // keep default value handling but prevent fields with null values from being serialized
            var jsonPropertyListing = JsonConvert.SerializeObject(propertyListingEnvelope, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            _logger.LogInformation($"Property {propertyListing.CommonPrimaryKey} - Json body : {jsonPropertyListing}");

            request.Content = new StringContent(jsonPropertyListing, Encoding.UTF8, "application/json");

            // Get response from Store Api
            HttpResponseMessage responseMessage = await _httpClient.SendAsync(request);

            // If response message return status code == 500, throw exception
            if (responseMessage.StatusCode == HttpStatusCode.InternalServerError)
            {
                var msg = $"Property {propertyListing.CommonPrimaryKey} - Failure to call store api endpoint.  Error message: {responseMessage.StatusCode}";
                _logger.LogError(msg);
                throw new StoreApiException(responseMessage.StatusCode.ToString(), msg);
            }

            // Get message from http response
            string storeApiResponse = await responseMessage.Content.ReadAsStringAsync();
            _logger.LogInformation($"Property {propertyListing.CommonPrimaryKey} - Response from store api: {storeApiResponse}");

            dynamic message = JsonConvert.DeserializeObject<dynamic>(storeApiResponse);
            if (message?.StoreApiStatusCode == StoreApiStatusEnum.Success)
            {
                return message.Data.ToString();
            }
            else
            {
                _logger.LogError($"Property {propertyListing.CommonPrimaryKey} - Failure to post data to store API.  Error message: {message.Message.ToString()}");
                throw new StoreApiException(message.StoreApiStatusCode.ToString(), message.Message.ToString());
            }
        }

        public Task<string> GetListing(PropertyListing propertyListing)
        {
            throw new NotImplementedException();
        }

        public async Task<string> RemoveListing(string primaryKey)
        {
            // Setup the url for the store api
            var request = new HttpRequestMessage(HttpMethod.Post, $"api/DeleteProperty/{primaryKey}?source=default");

            // Setup headers for the http message
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Set content for the http message
            string deleteBody = GetDeleteBody(primaryKey);
            request.Content = new StringContent(deleteBody, Encoding.UTF8, "application/json");

            // Get response from Store Api
            HttpResponseMessage responseMessage = await _httpClient.SendAsync(request);
            //responseMessage.EnsureSuccessStatusCode();

            // Get message from http response
            string storeApiResponse = await responseMessage.Content.ReadAsStringAsync();
            StoreApiResponseObject message = JsonConvert.DeserializeObject<StoreApiResponseObject>(storeApiResponse);
            if (message.StoreApiStatusCode == StoreApiStatusEnum.Success)
            {
                return message.Data;
            }
            else if (message.Message.Contains("not found for the entity"))
            {
                return "Listing Does Not Exist.";
            }
            else
            {
                throw new StoreApiException(message.StoreApiStatusCode.ToString(), message.Message);
            }
        }

        public async Task<string> HealthCheck()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/HealthCheck");
            HttpResponseMessage responseMessage = await _httpClient.SendAsync(request);
            var result = await responseMessage.Content.ReadAsStringAsync();

            return result;
        }
    }
}
