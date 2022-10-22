using dataentry.Extensions;
using dataentry.Services.Business.Listings;
using dataentry.Services.Business.Regions;
using dataentry.Services.Integration.StoreApi.Model;
using dataentry.ViewModels.GraphQL;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace dataentry.Services.Integration.SearchApi
{
    public class SearchApiService : ISearchApiService
    {
        private readonly HttpClient _httpClient;
        private readonly SearchApiServiceOptions _options;
        private readonly ILogger _logger;
        private readonly IListingService _listingService;
        private readonly IRegionService _regionService;
        private readonly IStoreApiListingMapper _mapper;

        public SearchApiService(
            HttpClient httpClient,
            IOptions<SearchApiServiceOptions> options,
            ILogger<SearchApiService> logger,
            IListingService listingService,
            IRegionService regionService,
            IStoreApiListingMapper mapper
        )
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _listingService = listingService ?? throw new ArgumentNullException(nameof(listingService));
            _regionService = regionService ?? throw new ArgumentNullException(nameof(regionService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        private static Regex TokenRegex = new Regex(@"^Documents\[\d+\]\[\d+\]$");

        public async Task<ImportListingViewModel> ImportListingsByQuery(
            ClaimsPrincipal user,
            string homeSiteId,
            string query,
            bool overwrite,
            List<string> assignToUsers,
            List<string> assignToTeams
        )
        {
            var region = await _regionService.GetRegionByHomeSiteId(homeSiteId);

            var request = new HttpRequestMessage(HttpMethod.Get, $"{_options.QueryEndpoint}?Site={homeSiteId}&{query}");

            // Setup headers for the http message
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var result = new ImportListingViewModel();
            var resultItems = new List<ImportListingDetailViewModel>();
            result.Listings = resultItems;

            HttpResponseMessage msg = await _httpClient.SendAsync(request);
            using (var contentStream = await msg.Content.ReadAsStreamAsync())
            using (var streamReader = new StreamReader(contentStream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                try
                {
                    foreach (var storeListing in jsonReader.SelectTokensWithRegex<PropertyListing>(TokenRegex))
                    {
                        var resultItem = new ImportListingDetailViewModel();
                        resultItems.Add(resultItem);

                        ListingViewModel vm = null;

                        try
                        {
                            vm = _mapper.Map(region, _options.ResourcesBaseUrl, storeListing);
                            if ((assignToUsers != null && assignToUsers.Any()) || assignToTeams != null && assignToTeams.Any())
                            {
                                vm.UserNames = assignToUsers ?? new List<string>();
                                vm.TeamNames = assignToTeams ?? new List<string>();
                            }
                            vm = await _listingService.CreateListingAsync(user, vm, imported: true);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(
                                ex,
                                "Error importing property {propertyId}",
                                new object[] { storeListing?.CommonPrimaryKey }
                            );

                            resultItem.ErrorMessage = BuildErrorMessage(ex);
                        }
                        resultItem.Id = vm?.Id ?? 0;
                        resultItem.ExternalId = storeListing.CommonPrimaryKey;
                    }
                }
                catch (Exception ex)
                {
                    result.ErrorMessage = BuildErrorMessage(ex);
                }
            }

            return result;
        }

        private static string BuildErrorMessage(Exception ex)
        {
            var exceptions = new List<Exception>();
            var i = 50;
            while (ex != null && i-- > 0)
            {
                exceptions.Add(ex);
                ex = ex.InnerException;
            }
            return $"{string.Join("\n-----\nInner Exception: ", exceptions.Select(ex => $"({ex.GetType()}) {ex.Message}"))}";
        }
    }
}
