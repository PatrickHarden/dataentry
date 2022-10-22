using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class updatePropertyAdditionalGeocodeMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"updatePropertyAdditionalGeocode"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"property_id", property_id);
                yield return new KeyValuePair<string, object>(@"addlgeocode", addlgeocode);
            }
        }

        public RequestDetails request { get; set; }

        public int property_id { get; set; }

        public PostalAddrGeocodeDetail addlgeocode { get; set; }
    }
}
