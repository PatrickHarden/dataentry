using System;
using System.Threading.Tasks;
using dataentry.Data.DBContext;
using dataentry.Repository;
using dataentry.ViewModels.GraphQL;
using Microsoft.Extensions.Configuration;

namespace dataentry.Services.Business.Configs
{
    public class ConfigService : IConfigService
    {
        private readonly IConfiguration _configuration;
        private readonly IDataEntryRepository _dataEntryRepository;

        public ConfigService(IConfiguration configuration, IDataEntryRepository dataEntryRepository)
        {
            _configuration = configuration ??
                throw new ArgumentNullException(nameof(configuration));
            _dataEntryRepository = dataEntryRepository ??
                throw new ArgumentNullException(nameof(dataEntryRepository));
        }

        public async Task<ConfigsViewModel> GetConfigs()
        {
            var defaultRegion = await _dataEntryRepository.GetDefaultRegion();
            return new ConfigsViewModel()
            {
                HomeSiteId = defaultRegion.HomeSiteID,
                AiKey = _configuration["APPINSIGHTS_INSTRUMENTATIONKEY"],
                PreviewFeatureFlag = _configuration["FeatureFlags:PreviewFeatureFlag"],
                WatermarkDetectionFeatureFlag = _configuration["FeatureFlags:WatermarkDetectionFeatureFlag"],
                MiqImportFeatureFlag = _configuration["FeatureFlags:MiqImportFeatureFlag"],
                MiqLimitSearchToCountryCodeFeatureFlag = _configuration["FeatureFlags:MiqLimitSearchToCountryCodeFeatureFlag"],
                GoogleMapsKey = _configuration["GoogleMapsAPI:Key"],
                GoogleMapsChannel = _configuration["GoogleMapsAPI:Channel"]
            };
        }

        public async Task<string> GetDefaultCultureCode()
        {
            var defaultRegion = await _dataEntryRepository.GetDefaultRegion();
            return defaultRegion.CultureCode;
        }
    }
}