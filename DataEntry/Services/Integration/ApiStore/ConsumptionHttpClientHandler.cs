using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace dataentry.Services.Integration.ApiStore
{
    public class ConsumptionHttpClientHandler : ApiStoreHttpClientHandler
    {
        public ConsumptionHttpClientHandler(IOptions<ConsumptionApiStoreOptions> options, ILogger<ApiStoreHttpClientHandler> log) : base(options, log){}
    }
}
