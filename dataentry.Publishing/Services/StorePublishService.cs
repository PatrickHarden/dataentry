using dataentry.Publishing.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace dataentry.Publishing.Services
{
    public class StorePublishService : IStorePublishService
    {
        private readonly string _homeSiteId;
        private readonly string _searchApiEndPoint;
        private readonly string _searchKey;
        private readonly string _listingOrigin;
        private readonly HttpClient _client;

        public StorePublishService()
        {
            _client = new HttpClient();
            _searchApiEndPoint = Environment.GetEnvironmentVariable("SearchApiEndPoint") ?? throw new ArgumentNullException("SearchApiEndPoint is null");
            _homeSiteId = Environment.GetEnvironmentVariable("HomeSiteId") ?? throw new ArgumentNullException("HomeSiteId is null");
            _searchKey = Environment.GetEnvironmentVariable("SearchKey") ?? throw new ArgumentNullException("SearchKey is null");
            _listingOrigin = Environment.GetEnvironmentVariable("ListingOrigin") ?? throw new ArgumentNullException("ListingOrigin is null");
        }

        /// <summary>
        /// This function will determine if a listing is in the desired state
        /// </summary>
        /// <param name="publishListing"></param>
        /// <param name="desireState"></param>
        /// <returns></returns>
        public async Task<bool> IsListingInDesiredStateAsync(PublishListing publishListing, PublishState desireState, ILogger log)
        {   
            // Build url for request
            var primaryKey = publishListing.ExternalID ?? $"{_listingOrigin}-{publishListing.Id}";
            string requestUrl = $"{_searchApiEndPoint}site={publishListing.HomeSiteID ?? _homeSiteId}&{_searchKey}={primaryKey}";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            log.LogInformation("Sending GET request to URL {0}", requestUrl);
            HttpResponseMessage responseMessage = await _client.SendAsync(request);
            if (responseMessage.IsSuccessStatusCode) 
            {
                var response = await responseMessage.Content.ReadAsStringAsync();

                dynamic data = JObject.Parse(response);

                if (data == null)
                {
                    throw new ArgumentNullException("Search Service response was null");
                }

                bool isFound = data?.Found == true && data?.DocumentCount > 0;

                if (desireState == PublishState.Published && isFound)
                    return true;

                if (desireState == PublishState.Unpublished && !isFound)
                    return true;
            }
            log.LogInformation("Response code was not success for URL {0}", requestUrl);

            return false;
        }
    }
}
