using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class GeographyUpdate : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{global_region_desc,region_desc,market_desc,submarket_desc,district_desc,neighborhood_desc}";

        [JsonProperty(@"global_region_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string global_region_desc { get; set; }

        [JsonProperty(@"region_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string region_desc { get; set; }

        [JsonProperty(@"market_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string market_desc { get; set; }

        [JsonProperty(@"submarket_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string submarket_desc { get; set; }

        [JsonProperty(@"district_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string district_desc { get; set; }

        [JsonProperty(@"neighborhood_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string neighborhood_desc { get; set; }
    }
}
