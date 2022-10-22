using System;
using System.Net.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace dataentry.Data
{
    public class SiteMapsConfigDataProvider : ISiteMapsConfigDataProvider
    {
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5);
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly string _siteMapsConfigUrl;
        private readonly ILogger<SiteMapsConfigDataProvider> _logger;

        public SiteMapsConfigDataProvider(HttpClient httpClient, IConfiguration configuration, IMemoryCache cache, ILogger<SiteMapsConfigDataProvider> logger)
        {
            _httpClient = httpClient ??
                throw new ArgumentNullException(nameof(httpClient));
            _siteMapsConfigUrl = configuration["SiteMapsConfig"] ??
                throw new ArgumentException("SiteMapsConfig setting is null");
            _cache = cache ??
                throw new ArgumentNullException(nameof(cache));
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        public JObject GetSitemapConfig()
        {
            try
            {
                return _cache.GetOrCreate("SiteMapsConfigDataProvider", cacheEntry =>
                {
                    cacheEntry.AbsoluteExpirationRelativeToNow = _cacheDuration;
                    return JsonConvert.DeserializeObject<JObject>(_httpClient.GetStringAsync(_siteMapsConfigUrl).Result);
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error fetching sitemap config.");
                return null;
            }
        }
    }
}