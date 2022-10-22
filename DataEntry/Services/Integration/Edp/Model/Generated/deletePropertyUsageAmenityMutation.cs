using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class deletePropertyUsageAmenityMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"deletePropertyUsageAmenity"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"property_id", property_id);
                yield return new KeyValuePair<string, object>(@"property_usage_id", property_usage_id);
                yield return new KeyValuePair<string, object>(@"property_amenity_id", property_amenity_id);
            }
        }

        public RequestDetails request { get; set; }

        public int property_id { get; set; }

        public int property_usage_id { get; set; }

        public int property_amenity_id { get; set; }
    }
}
