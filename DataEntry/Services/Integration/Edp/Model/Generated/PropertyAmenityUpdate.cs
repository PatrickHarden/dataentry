using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class PropertyAmenityUpdate : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,amenity_name,amenity_notes,amenity_desc,amenity_distance_from_property,amenity_distance_from_property_uom,amenity_purchase_price,amenity_purchase_price_uom,size,colloquial{id,amenity_name,size,amenity_desc,amenity_notes}}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public int? id { get; set; }

        [JsonProperty(@"amenity_name", NullValueHandling = NullValueHandling.Ignore)]
        public string amenity_name { get; set; }

        [JsonProperty(@"amenity_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string amenity_notes { get; set; }

        [JsonProperty(@"amenity_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string amenity_desc { get; set; }

        [JsonProperty(@"amenity_distance_from_property", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? amenity_distance_from_property { get; set; }

        [JsonProperty(@"amenity_distance_from_property_uom", NullValueHandling = NullValueHandling.Ignore)]
        public string amenity_distance_from_property_uom { get; set; }

        [JsonProperty(@"amenity_purchase_price", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? amenity_purchase_price { get; set; }

        [JsonProperty(@"amenity_purchase_price_uom", NullValueHandling = NullValueHandling.Ignore)]
        public string amenity_purchase_price_uom { get; set; }

        [JsonProperty(@"size", NullValueHandling = NullValueHandling.Ignore)]
        public string size { get; set; }

        [JsonProperty(@"colloquial", NullValueHandling = NullValueHandling.Ignore)]
        public List<PropertyAmenityColloquialUpdate> colloquial { get; set; }
    }
}
