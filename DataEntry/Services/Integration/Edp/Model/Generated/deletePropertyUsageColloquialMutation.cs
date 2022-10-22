using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class deletePropertyUsageColloquialMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"deletePropertyUsageColloquial"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"property_id", property_id);
                yield return new KeyValuePair<string, object>(@"property_usage_id", property_usage_id);
                yield return new KeyValuePair<string, object>(@"usage_colloquial_id", usage_colloquial_id);
            }
        }

        public RequestDetails request { get; set; }

        public int property_id { get; set; }

        public int property_usage_id { get; set; }

        public int usage_colloquial_id { get; set; }
    }
}
