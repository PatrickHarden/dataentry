using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class PropertyApnNumber : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,property_apn_number}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public int? id { get; set; }

        [JsonProperty(@"property_apn_number", Required = Required.Always)]
        public string property_apn_number { get; set; }
    }
}
