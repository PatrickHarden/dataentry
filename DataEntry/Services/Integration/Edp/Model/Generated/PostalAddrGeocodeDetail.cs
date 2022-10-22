using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class PostalAddrGeocodeDetail : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,longitude,latitude}";

        [JsonProperty(@"id", Required = Required.Always)]
        public int id { get; set; }

        [JsonProperty(@"longitude", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? longitude { get; set; }

        [JsonProperty(@"latitude", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? latitude { get; set; }
    }
}
