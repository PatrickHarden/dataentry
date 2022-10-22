using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class PropertyExternalRating : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,green_building_rating_type_desc,green_building_category_desc,green_building_cert_level_desc,green_building_cert_date,year_certified}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public int? id { get; set; }

        [JsonProperty(@"green_building_rating_type_desc", Required = Required.Always)]
        public string green_building_rating_type_desc { get; set; }

        [JsonProperty(@"green_building_category_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string green_building_category_desc { get; set; }

        [JsonProperty(@"green_building_cert_level_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string green_building_cert_level_desc { get; set; }

        [JsonProperty(@"green_building_cert_date", NullValueHandling = NullValueHandling.Ignore)]
        public string green_building_cert_date { get; set; }

        [JsonProperty(@"year_certified", NullValueHandling = NullValueHandling.Ignore)]
        public int? year_certified { get; set; }
    }
}
