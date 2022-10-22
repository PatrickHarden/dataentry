using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class PropertySpace : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{floor_index,floor_id,floor_number,floor_desc,hide_floor_f,floor_size,floor_size_uom_desc,floor_plan_id,floor_plan_url,suite_id,suite_number,suite_desc,non_rentable_space{space_classification_type_desc,non_rentable_size,non_rentable_size_uom_desc,notes}}";

        [JsonProperty(@"floor_index", NullValueHandling = NullValueHandling.Ignore)]
        public int? floor_index { get; set; }

        [JsonProperty(@"floor_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? floor_id { get; set; }

        [JsonProperty(@"floor_number", NullValueHandling = NullValueHandling.Ignore)]
        public string floor_number { get; set; }

        [JsonProperty(@"floor_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string floor_desc { get; set; }

        [JsonProperty(@"hide_floor_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? hide_floor_f { get; set; }

        [JsonProperty(@"floor_size", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? floor_size { get; set; }

        [JsonProperty(@"floor_size_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string floor_size_uom_desc { get; set; }

        [JsonProperty(@"floor_plan_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? floor_plan_id { get; set; }

        [JsonProperty(@"floor_plan_url", NullValueHandling = NullValueHandling.Ignore)]
        public string floor_plan_url { get; set; }

        [JsonProperty(@"suite_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? suite_id { get; set; }

        [JsonProperty(@"suite_number", NullValueHandling = NullValueHandling.Ignore)]
        public string suite_number { get; set; }

        [JsonProperty(@"suite_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string suite_desc { get; set; }

        [JsonProperty(@"non_rentable_space", NullValueHandling = NullValueHandling.Ignore)]
        public NonRentableSpace non_rentable_space { get; set; }
    }
}
