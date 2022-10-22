using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class deletePropertyLandlordProvisionColloquialMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"deletePropertyLandlordProvisionColloquial"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"property_id", property_id);
                yield return new KeyValuePair<string, object>(@"property_landlord_provision_id", property_landlord_provision_id);
                yield return new KeyValuePair<string, object>(@"colloquial_id", colloquial_id);
            }
        }

        public RequestDetails request { get; set; }

        public int property_id { get; set; }

        public int property_landlord_provision_id { get; set; }

        public int colloquial_id { get; set; }
    }
}
