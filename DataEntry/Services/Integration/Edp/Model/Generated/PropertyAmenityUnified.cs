using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class PropertyAmenityUnified : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,property_amenity_type_desc,amenity_desc,amenity_notes,amenity_distance_from_property,amenity_distance_from_property_uom,amenity_purchase_price,amenity_purchase_price_uom}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public string id { get; set; }

        [JsonProperty(@"property_amenity_type_desc", Required = Required.Always)]
        public string property_amenity_type_desc { get; set; }

        [JsonProperty(@"amenity_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string amenity_desc { get; set; }

        [JsonProperty(@"amenity_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string amenity_notes { get; set; }

        [JsonProperty(@"amenity_distance_from_property", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? amenity_distance_from_property { get; set; }

        [JsonProperty(@"amenity_distance_from_property_uom", NullValueHandling = NullValueHandling.Ignore)]
        public string amenity_distance_from_property_uom { get; set; }

        [JsonProperty(@"amenity_purchase_price", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? amenity_purchase_price { get; set; }

        [JsonProperty(@"amenity_purchase_price_uom", NullValueHandling = NullValueHandling.Ignore)]
        public string amenity_purchase_price_uom { get; set; }
    }
}
