using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class Neighborhood : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{neighborhood_id,neighborhood_desc}";

        [JsonProperty(@"neighborhood_id", NullValueHandling = NullValueHandling.Ignore)]
        public string neighborhood_id { get; set; }

        [JsonProperty(@"neighborhood_desc", Required = Required.Always)]
        public string neighborhood_desc { get; set; }
    }
}
