using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class Region : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{region_id,region_desc,market{market_id,market_desc,submarket{submarket_id,submarket_desc,district{district_id,district_desc,neighborhood{neighborhood_id,neighborhood_desc}}}}}";

        [JsonProperty(@"region_id", NullValueHandling = NullValueHandling.Ignore)]
        public string region_id { get; set; }

        [JsonProperty(@"region_desc", Required = Required.Always)]
        public string region_desc { get; set; }

        [JsonProperty(@"market", NullValueHandling = NullValueHandling.Ignore)]
        public List<Market> market { get; set; }
    }
}
