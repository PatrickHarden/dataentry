using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class updateBrandMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"updateBrand"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"brand_id", brand_id);
                yield return new KeyValuePair<string, object>(@"brand_update", brand_update);
            }
        }

        public RequestDetails request { get; set; }

        public int brand_id { get; set; }

        public BrandUpdate brand_update { get; set; }
    }
}
