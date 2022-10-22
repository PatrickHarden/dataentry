using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class PropertyFrontage : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,frontage_length,frontage_length_uom_desc,frontage_street_name,frontage_street_type}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public int? id { get; set; }

        [JsonProperty(@"frontage_length", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? frontage_length { get; set; }

        [JsonProperty(@"frontage_length_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string frontage_length_uom_desc { get; set; }

        [JsonProperty(@"frontage_street_name", Required = Required.Always)]
        public string frontage_street_name { get; set; }

        [JsonProperty(@"frontage_street_type", NullValueHandling = NullValueHandling.Ignore)]
        public string frontage_street_type { get; set; }
    }
}
