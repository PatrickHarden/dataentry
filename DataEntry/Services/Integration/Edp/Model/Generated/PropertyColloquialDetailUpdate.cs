using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class PropertyColloquialDetailUpdate : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,property_name,property_desc,property_type_desc,parking_comment}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public int? id { get; set; }

        [JsonProperty(@"property_name", NullValueHandling = NullValueHandling.Ignore)]
        public string property_name { get; set; }

        [JsonProperty(@"property_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string property_desc { get; set; }

        [JsonProperty(@"property_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string property_type_desc { get; set; }

        [JsonProperty(@"parking_comment", NullValueHandling = NullValueHandling.Ignore)]
        public string parking_comment { get; set; }
    }
}
