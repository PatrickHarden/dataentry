using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class City : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,city_desc}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public int? id { get; set; }

        [JsonProperty(@"city_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string city_desc { get; set; }
    }
}
