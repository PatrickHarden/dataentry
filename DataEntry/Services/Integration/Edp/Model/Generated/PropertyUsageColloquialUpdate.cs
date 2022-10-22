using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class PropertyUsageColloquialUpdate : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,additional_property_usage_note}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public int? id { get; set; }

        [JsonProperty(@"additional_property_usage_note", NullValueHandling = NullValueHandling.Ignore)]
        public string additional_property_usage_note { get; set; }
    }
}
