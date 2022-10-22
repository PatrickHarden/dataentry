using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class AssetColloquialUpdate : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,asset_name,asset_desc}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public int? id { get; set; }

        [JsonProperty(@"asset_name", NullValueHandling = NullValueHandling.Ignore)]
        public string asset_name { get; set; }

        [JsonProperty(@"asset_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string asset_desc { get; set; }
    }
}
