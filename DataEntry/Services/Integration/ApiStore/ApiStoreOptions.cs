using System;
using System.Collections.Generic;
using System.Text;

namespace dataentry.Services.Integration.ApiStore
{
    public class ConsumptionApiStoreOptions: ApiStoreOptions{}
    public class IngestionApiStoreOptions: ApiStoreOptions{}

    public class ApiStoreOptions
    {
        public bool Enabled { get; set; }
        public Uri Url { get; set; }
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }

    }
}
