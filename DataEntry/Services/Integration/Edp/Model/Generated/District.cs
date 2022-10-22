using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class District : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{district_id,district_desc,neighborhood{neighborhood_id,neighborhood_desc}}";

        [JsonProperty(@"district_id", NullValueHandling = NullValueHandling.Ignore)]
        public string district_id { get; set; }

        [JsonProperty(@"district_desc", Required = Required.Always)]
        public string district_desc { get; set; }

        [JsonProperty(@"neighborhood", NullValueHandling = NullValueHandling.Ignore)]
        public List<Neighborhood> neighborhood { get; set; }
    }
}
