using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class addPropertyAssetColloquialMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"addPropertyAssetColloquial"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"property_id", property_id);
                yield return new KeyValuePair<string, object>(@"asset_id", asset_id);
                yield return new KeyValuePair<string, object>(@"colloquial", colloquial);
            }
        }

        public RequestDetails request { get; set; }

        public int property_id { get; set; }

        public int asset_id { get; set; }

        public AssetColloquial colloquial { get; set; }
    }
}
