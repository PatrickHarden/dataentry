using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class AssetUpdate : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,asset_name,asset_desc,asset_capacity,asset_capacity_uom,colloquial{id,asset_name,asset_desc}}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public int? id { get; set; }

        [JsonProperty(@"asset_name", NullValueHandling = NullValueHandling.Ignore)]
        public string asset_name { get; set; }

        [JsonProperty(@"asset_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string asset_desc { get; set; }

        [JsonProperty(@"asset_capacity", NullValueHandling = NullValueHandling.Ignore)]
        public int? asset_capacity { get; set; }

        [JsonProperty(@"asset_capacity_uom", NullValueHandling = NullValueHandling.Ignore)]
        public string asset_capacity_uom { get; set; }

        [JsonProperty(@"colloquial", NullValueHandling = NullValueHandling.Ignore)]
        public List<AssetColloquialUpdate> colloquial { get; set; }
    }
}
