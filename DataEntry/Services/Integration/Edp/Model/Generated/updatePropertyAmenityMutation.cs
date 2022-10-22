using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class updatePropertyAmenityMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"updatePropertyAmenity"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"property_id", property_id);
                yield return new KeyValuePair<string, object>(@"amenity_id", amenity_id);
                yield return new KeyValuePair<string, object>(@"amenity", amenity);
            }
        }

        public RequestDetails request { get; set; }

        public int property_id { get; set; }

        public int amenity_id { get; set; }

        public PropertyAmenityUpdate amenity { get; set; }
    }
}
