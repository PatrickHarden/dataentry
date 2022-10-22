using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class PropertyAmenityColloquialUpdate : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,amenity_name,size,amenity_desc,amenity_notes}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public int? id { get; set; }

        [JsonProperty(@"amenity_name", NullValueHandling = NullValueHandling.Ignore)]
        public string amenity_name { get; set; }

        [JsonProperty(@"size", NullValueHandling = NullValueHandling.Ignore)]
        public string size { get; set; }

        [JsonProperty(@"amenity_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string amenity_desc { get; set; }

        [JsonProperty(@"amenity_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string amenity_notes { get; set; }
    }
}
