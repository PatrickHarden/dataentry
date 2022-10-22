using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class AvailabilityPropertyAssociation : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{property_id,property_usage_id,floor_suite{floor_number,suite_number}}";

        [JsonProperty(@"property_id", Required = Required.Always)]
        public int property_id { get; set; }

        [JsonProperty(@"property_usage_id", Required = Required.Always)]
        public int property_usage_id { get; set; }

        [JsonProperty(@"floor_suite", NullValueHandling = NullValueHandling.Ignore)]
        public FloorNSuite floor_suite { get; set; }
    }
}
