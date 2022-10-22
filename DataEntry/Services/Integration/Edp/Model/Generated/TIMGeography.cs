using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class TIMGeography : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{region{region_id,region_desc,market{market_id,market_desc,submarket{submarket_id,submarket_desc,district{district_id,district_desc,neighborhood{neighborhood_id,neighborhood_desc}}}}},lig_geography{id,global_region_id,global_region_desc,division_id,division_desc,market_area_id,market_area_desc,local_market_area_id,local_market_area_desc,market_id,market_desc,submarket_id,submarket_desc,district_id,district_desc,neighborhood_id,neighborhood_desc},city{id,city_desc}}";

        [JsonProperty(@"region", NullValueHandling = NullValueHandling.Ignore)]
        public List<Region> region { get; set; }

        [JsonProperty(@"lig_geography", NullValueHandling = NullValueHandling.Ignore)]
        public List<LIGGeography> lig_geography { get; set; }

        [JsonProperty(@"city", NullValueHandling = NullValueHandling.Ignore)]
        public List<City> city { get; set; }
    }
}
