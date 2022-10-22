using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class LIGGeography : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,global_region_id,global_region_desc,division_id,division_desc,market_area_id,market_area_desc,local_market_area_id,local_market_area_desc,market_id,market_desc,submarket_id,submarket_desc,district_id,district_desc,neighborhood_id,neighborhood_desc}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public int? id { get; set; }

        [JsonProperty(@"global_region_id", NullValueHandling = NullValueHandling.Ignore)]
        public string global_region_id { get; set; }

        [JsonProperty(@"global_region_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string global_region_desc { get; set; }

        [JsonProperty(@"division_id", NullValueHandling = NullValueHandling.Ignore)]
        public string division_id { get; set; }

        [JsonProperty(@"division_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string division_desc { get; set; }

        [JsonProperty(@"market_area_id", NullValueHandling = NullValueHandling.Ignore)]
        public string market_area_id { get; set; }

        [JsonProperty(@"market_area_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string market_area_desc { get; set; }

        [JsonProperty(@"local_market_area_id", NullValueHandling = NullValueHandling.Ignore)]
        public string local_market_area_id { get; set; }

        [JsonProperty(@"local_market_area_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string local_market_area_desc { get; set; }

        [JsonProperty(@"market_id", NullValueHandling = NullValueHandling.Ignore)]
        public string market_id { get; set; }

        [JsonProperty(@"market_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string market_desc { get; set; }

        [JsonProperty(@"submarket_id", NullValueHandling = NullValueHandling.Ignore)]
        public string submarket_id { get; set; }

        [JsonProperty(@"submarket_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string submarket_desc { get; set; }

        [JsonProperty(@"district_id", NullValueHandling = NullValueHandling.Ignore)]
        public string district_id { get; set; }

        [JsonProperty(@"district_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string district_desc { get; set; }

        [JsonProperty(@"neighborhood_id", NullValueHandling = NullValueHandling.Ignore)]
        public string neighborhood_id { get; set; }

        [JsonProperty(@"neighborhood_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string neighborhood_desc { get; set; }
    }
}
