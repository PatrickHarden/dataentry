using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class TIMUsages : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,ref_property_usage_type_desc,ref_property_usage_sub_type_desc,ref_property_class_type_desc}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public int? id { get; set; }

        [JsonProperty(@"ref_property_usage_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_property_usage_type_desc { get; set; }

        [JsonProperty(@"ref_property_usage_sub_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_property_usage_sub_type_desc { get; set; }

        [JsonProperty(@"ref_property_class_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_property_class_type_desc { get; set; }
    }
}
