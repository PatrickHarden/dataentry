using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class updatePropertyUsageMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"updatePropertyUsage"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"property_id", property_id);
                yield return new KeyValuePair<string, object>(@"property_usage_id", property_usage_id);
                yield return new KeyValuePair<string, object>(@"property_usage", property_usage);
            }
        }

        public RequestDetails request { get; set; }

        public int property_id { get; set; }

        public int property_usage_id { get; set; }

        public PropertyUsageUpdate property_usage { get; set; }
    }
}
