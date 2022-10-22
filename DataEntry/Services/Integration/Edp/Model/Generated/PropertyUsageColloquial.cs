using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class PropertyUsageColloquial : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,additional_property_usage_note,country_code_desc,language_desc}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public int? id { get; set; }

        [JsonProperty(@"additional_property_usage_note", NullValueHandling = NullValueHandling.Ignore)]
        public string additional_property_usage_note { get; set; }

        [JsonProperty(@"country_code_desc", Required = Required.Always)]
        public string country_code_desc { get; set; }

        [JsonProperty(@"language_desc", Required = Required.Always)]
        public string language_desc { get; set; }
    }
}
