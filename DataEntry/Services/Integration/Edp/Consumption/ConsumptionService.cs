using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using dataentry.Repository;
using dataentry.Services.Business.Images;
using dataentry.Services.Business.Listings;
using dataentry.ViewModels.GraphQL;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace dataentry.Services.Integration.Edp.Consumption
{
    public class ConsumptionService : IConsumptionService
    {
        public bool Enabled => _consumptionOptions?.Enabled ?? false;
        private readonly IDataEntryRepository _dataEntryRepository;
        private readonly EdpGraphQLService _edpGraphQLService;
        private readonly IPropertyMapper _propertyMapper;
        private readonly IListingMapper _listingMapper;
        private readonly IFileService _fileService;
        private readonly ILogger<EdpGraphQLService> _log;
        private readonly ConsumptionOptions _consumptionOptions;
        private readonly ConsumptionQueries _consumptionQueries;

        public ConsumptionService(IDataEntryRepository dataEntryRepository, HttpClient httpClient, IOptions<ConsumptionOptions> edpOptions, IPropertyMapper propertyMapper, IListingMapper listingMapper, IFileService fileService, ILogger<EdpGraphQLService> log)
        {
            _dataEntryRepository = dataEntryRepository ??
                throw new ArgumentNullException(nameof(dataEntryRepository));
            _propertyMapper = propertyMapper ??
                throw new ArgumentNullException(nameof(propertyMapper));
            _listingMapper = listingMapper ??
                throw new ArgumentNullException(nameof(listingMapper));
            _fileService = fileService ??
                throw new ArgumentNullException(nameof(fileService));
            _log = log ??
                throw new ArgumentNullException(nameof(log));
            _edpGraphQLService = new EdpGraphQLService(httpClient, edpOptions, log);
            _consumptionOptions = edpOptions?.Value ??
                throw new ArgumentNullException(nameof(edpOptions));
            _consumptionQueries = new ConsumptionQueries(_consumptionOptions);
        }

        public async Task<IEnumerable<PropertySearchResultViewModel>> SearchProperties(string keyword, string country)
        {
            if (!Enabled) ThrowDisabledException();
            string requestId = Guid.NewGuid().ToString();
            var data = await _edpGraphQLService.RunQuery(_consumptionQueries.GetSearchPropertyQuery(keyword, country, requestId));
            var results = JsonConvert.DeserializeObject<PropertySearchResults>(data);
            _log.LogInformation($"{requestId} - Number of results search by '{keyword}' : {results?.data?.searchProperty?.totalCount ?? 0}");
            return results.data?.searchProperty?.properties?.Select(x => _propertyMapper.Map(x));
        }

        public async Task<ListingViewModel> GetListingByMiqId(int id, Guid regionId, bool serializeFileData)
        {
            if (!Enabled) ThrowDisabledException();
            
            var property = await GetPropertyWithAvailability(id, regionId);
            var listing = await ConvertPropertyWithAvailabilityToListingViewModel(property, regionId);
            if (serializeFileData) await ConvertFilesToBase64String(listing);
            return listing;
        }

        public async Task<ListingViewModel> ConvertPropertyWithAvailabilityToListingViewModel(PropertyWithAvailability property, Guid regionId)
        {
            if (!Enabled) ThrowDisabledException();

            var region = await _dataEntryRepository.GetRegionById(regionId) ?? await _dataEntryRepository.GetDefaultRegion();
            var listing = _propertyMapper.ConvertToListing(property, region);

            if (listing == null) // ID does not exist in MIQ
            {
                return null;
            }

            // Use existing broker data if it exists
            var brokers = await _dataEntryRepository.GetBrokersByEmails(listing.Contacts.Select(c => c.Email));
            listing.Contacts = listing.Contacts.Select(c =>
            {
                var broker = brokers.FirstOrDefault(b => b.Email.Equals(c.Email, StringComparison.OrdinalIgnoreCase));
                return broker == null ? c : _listingMapper.Map(broker);
            }).ToList();

            return listing;
        }

        public async Task<PropertyWithAvailability> GetPropertyWithAvailability(int id, Guid regionId)
        {
            if (!Enabled) ThrowDisabledException();

            var converters = new JsonConverter[] { new CustomIsoDateTimeConverter() };
            string requestId = Guid.NewGuid().ToString();
            var propertyDetailData = await _edpGraphQLService.RunQuery(_consumptionQueries.GetPropertyQuery(id, requestId));
            var propertyDetail = JsonConvert.DeserializeObject<PropertyDetailResult>(propertyDetailData, converters);

            var availabilityResultData = await _edpGraphQLService.RunQuery(_consumptionQueries.GetAvailabilityQuery(id, requestId));
            var availabilityResult = JsonConvert.DeserializeObject<AvailbityDetailResult>(availabilityResultData, converters);

            PropertyWithAvailability property = new PropertyWithAvailability();
            property.PropertyDetail = propertyDetail?.data?.searchProperty?.properties?.FirstOrDefault();
            property.Availability = availabilityResult?.data?.searchAvailability?.availability;

            return property;
        }

        private async Task ConvertFilesToBase64String(ListingViewModel vm)
        {
            if (vm == null) return;
            await Task.WhenAll(vm.Photos?.Select(async x => x.Base64String = await _fileService.GetFileBinary(x.Url)));
            await Task.WhenAll(vm.Floorplans?.Select(async x => x.Base64String = await _fileService.GetFileBinary(x.Url)));
            await Task.WhenAll(vm.Brochures?.Select(async x => x.Base64String = await _fileService.GetFileBinary(x.Url)));
            await Task.WhenAll(vm.Spaces?.SelectMany(s => s.Photos)?.Select(async x => x.Base64String = await _fileService.GetFileBinary(x.Url)));
            await Task.WhenAll(vm.Spaces?.SelectMany(s => s.Floorplans)?.Select(async x => x.Base64String = await _fileService.GetFileBinary(x.Url)));
            await Task.WhenAll(vm.Spaces?.SelectMany(s => s.Brochures)?.Select(async x => x.Base64String = await _fileService.GetFileBinary(x.Url)));

            vm.Photos = vm.Photos?.Where(x => !string.IsNullOrWhiteSpace(x.Base64String));
            vm.Floorplans = vm.Floorplans?.Where(x => !string.IsNullOrWhiteSpace(x.Base64String));
            vm.Brochures = vm.Brochures?.Where(x => !string.IsNullOrWhiteSpace(x.Base64String));
            if (vm.Spaces != null)
            {
                foreach (var space in vm.Spaces)
                {
                    space.Photos = space.Photos?.Where(x => !string.IsNullOrWhiteSpace(x.Base64String));
                    space.Floorplans = space.Floorplans?.Where(x => !string.IsNullOrWhiteSpace(x.Base64String));
                    space.Brochures = space.Brochures?.Where(x => !string.IsNullOrWhiteSpace(x.Base64String));
                }
            }
        }

        private void ThrowDisabledException()
        {
            throw new InvalidOperationException("ConsumptionService is not enabled.");
        }
    }
}