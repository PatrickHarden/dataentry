using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class SalePropertyAssociation : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{property_id,property_usage_id}";

        [JsonProperty(@"property_id", Required = Required.Always)]
        public int property_id { get; set; }

        [JsonProperty(@"property_usage_id", Required = Required.Always)]
        public int property_usage_id { get; set; }
    }
}
