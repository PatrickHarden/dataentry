using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class ListingID : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{listing_id,listing_source}";

        [JsonProperty(@"listing_id", Required = Required.Always)]
        public string listing_id { get; set; }

        [JsonProperty(@"listing_source", Required = Required.Always)]
        public string listing_source { get; set; }
    }
}
