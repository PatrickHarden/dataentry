using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class SubMarket : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{submarket_id,submarket_desc,district{district_id,district_desc,neighborhood{neighborhood_id,neighborhood_desc}}}";

        [JsonProperty(@"submarket_id", NullValueHandling = NullValueHandling.Ignore)]
        public string submarket_id { get; set; }

        [JsonProperty(@"submarket_desc", Required = Required.Always)]
        public string submarket_desc { get; set; }

        [JsonProperty(@"district", NullValueHandling = NullValueHandling.Ignore)]
        public List<District> district { get; set; }
    }
}
