using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class Asset : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,asset_name,asset_type_desc,ref_asset_sub_type_code,asset_sub_type_desc,asset_desc,asset_capacity,asset_capacity_uom,colloquial{id,asset_name,asset_desc,country_code_desc,language_desc}}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public int? id { get; set; }

        [JsonProperty(@"asset_name", NullValueHandling = NullValueHandling.Ignore)]
        public string asset_name { get; set; }

        [JsonProperty(@"asset_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string asset_type_desc { get; set; }

        [JsonProperty(@"ref_asset_sub_type_code", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_asset_sub_type_code { get; set; }

        [JsonProperty(@"asset_sub_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string asset_sub_type_desc { get; set; }

        [JsonProperty(@"asset_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string asset_desc { get; set; }

        [JsonProperty(@"asset_capacity", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? asset_capacity { get; set; }

        [JsonProperty(@"asset_capacity_uom", NullValueHandling = NullValueHandling.Ignore)]
        public string asset_capacity_uom { get; set; }

        [JsonProperty(@"colloquial", NullValueHandling = NullValueHandling.Ignore)]
        public List<AssetColloquial> colloquial { get; set; }
    }
}
