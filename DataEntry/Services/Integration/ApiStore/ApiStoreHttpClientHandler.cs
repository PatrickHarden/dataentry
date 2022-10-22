using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace dataentry.Services.Integration.ApiStore
{
    public class ApiStoreHttpClientHandler : DelegatingHandler
    {
        private SemaphoreSlim _tokenLock = new SemaphoreSlim(1, 1);
        private HttpClient _tokenClient;
        private string _token;
        private DateTime _tokenExpiry;
        private ApiStoreOptions _options;
        private ILogger<ApiStoreHttpClientHandler> _log;

        public ApiStoreHttpClientHandler(IOptions<ApiStoreOptions> options, ILogger<ApiStoreHttpClientHandler> log)
        {
            _options = options?.Value ??
                throw new ArgumentNullException(nameof(options));
            _log = log ??
                throw new ArgumentNullException(nameof(log));

            if (_options.Enabled)
            {
                if (_options.Url == null) LogNullConfigWarning("ApiStore:Url");
                if (string.IsNullOrWhiteSpace(_options.ConsumerKey)) LogNullConfigWarning("ApiStore:ConsumerKey");
                if (string.IsNullOrWhiteSpace(_options.ConsumerSecret)) LogNullConfigWarning("ApiStore:ConsumerSecret");
            }

            _tokenClient = CreateTokenClient();
            InnerHandler = new HttpClientHandler();

            _log.LogDebug("Initialized ApiStoreHttpClientHandler");
        }

        private void LogNullConfigWarning(string configKey)
        {
            _log.LogWarning("Config value is null: {configKey}", configKey);
        }

        protected override void Dispose(bool disposing)
        {
            _log.LogDebug("Disposing ApiStoreHttpClientHandler", disposing);
            base.Dispose(disposing);
            _tokenClient.Dispose();
            _tokenLock.Dispose();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("Authorization", $"Bearer {await GetToken()}");
            return await base.SendAsync(request, cancellationToken);
        }

        private HttpClient CreateTokenClient()
        {
            var client = new HttpClient();
            client.BaseAddress = _options.Url;
            var encodedCredentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{_options.ConsumerKey}:{_options.ConsumerSecret}"));
            client.DefaultRequestHeaders.Add("Authorization", $"Basic {encodedCredentials}");
            client.DefaultRequestHeaders.Add("cache-control", "no-cache");

            return client;
        }

        private bool TokenIsStale => _token == null || _tokenExpiry < DateTime.UtcNow;

        private async Task<string> GetToken()
        {
            if (TokenIsStale)
            {
                await _tokenLock.WaitAsync();
                try
                {
                    if (TokenIsStale)
                    {
                        _log.LogDebug("ApiStoreHttpClientHandler requesting new token.");

                        var now = DateTime.UtcNow;
                        HttpResponseMessage response;
                        response = await _tokenClient.PostAsync("token", new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded"));
                        if (!response.IsSuccessStatusCode)
                        {
                            throw new ApiStoreTokenException(response);
                        }
                        var responseString = await response.Content.ReadAsStringAsync();
                        var tokenMessage = JsonConvert.DeserializeObject<TokenMessage>(responseString);
                        _token = tokenMessage.access_token;
                        _tokenExpiry = now.AddSeconds(tokenMessage.expires_in);

                        _log.LogDebug("ApiStoreHttpClientHandler received new token. Expires at {0}", _tokenExpiry);
                    }
                }
                finally
                {
                    _tokenLock.Release();
                }
            }
            return _token;
        }
    }
    public class ApiStoreTokenException : Exception
    {
        public ApiStoreTokenException(Exception inner) : base("Failed to request new API Store token, see inner exception for details.", inner) { }
        public ApiStoreTokenException(HttpResponseMessage response) : base(BuildMessage(response)) { }

        private static string BuildMessage(HttpResponseMessage response)
        {
            string content = string.Empty;
            if (response.Content != null)
            {
                using(var reader = new StreamReader(response.Content.ReadAsStream()))
                {
                    content = reader.ReadToEnd();
                }
            }
            return $"Failed to request new API Store Token: Received error response from API Store:\n({response.StatusCode}) {response.ReasonPhrase}\n{content}";
        }
    }
}