using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class updateBrandMediaInformationMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"updateBrandMediaInformation"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"brand_id", brand_id);
                yield return new KeyValuePair<string, object>(@"brand_media_information_id", brand_media_information_id);
                yield return new KeyValuePair<string, object>(@"brand_media_information", brand_media_information);
            }
        }

        public RequestDetails request { get; set; }

        public int brand_id { get; set; }

        public int brand_media_information_id { get; set; }

        public BrandMediaInformationUpdate brand_media_information { get; set; }
    }
}
