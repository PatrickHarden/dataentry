using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class Market : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{market_id,market_desc,submarket{submarket_id,submarket_desc,district{district_id,district_desc,neighborhood{neighborhood_id,neighborhood_desc}}}}";

        [JsonProperty(@"market_id", NullValueHandling = NullValueHandling.Ignore)]
        public string market_id { get; set; }

        [JsonProperty(@"market_desc", Required = Required.Always)]
        public string market_desc { get; set; }

        [JsonProperty(@"submarket", NullValueHandling = NullValueHandling.Ignore)]
        public List<SubMarket> submarket { get; set; }
    }
}
