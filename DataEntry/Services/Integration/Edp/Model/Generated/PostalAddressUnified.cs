using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class PostalAddressUnified : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,street1,street2,street_name,pre_street_direction_name,street_type,post_street_direction_name,address_line_1,city,state_province,postal_code,postal_code_extension,county,country,latitude,longitude}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public string id { get; set; }

        [JsonProperty(@"street1", NullValueHandling = NullValueHandling.Ignore)]
        public string street1 { get; set; }

        [JsonProperty(@"street2", NullValueHandling = NullValueHandling.Ignore)]
        public string street2 { get; set; }

        [JsonProperty(@"street_name", NullValueHandling = NullValueHandling.Ignore)]
        public string street_name { get; set; }

        [JsonProperty(@"pre_street_direction_name", NullValueHandling = NullValueHandling.Ignore)]
        public string pre_street_direction_name { get; set; }

        [JsonProperty(@"street_type", NullValueHandling = NullValueHandling.Ignore)]
        public string street_type { get; set; }

        [JsonProperty(@"post_street_direction_name", NullValueHandling = NullValueHandling.Ignore)]
        public string post_street_direction_name { get; set; }

        [JsonProperty(@"address_line_1", NullValueHandling = NullValueHandling.Ignore)]
        public string address_line_1 { get; set; }

        [JsonProperty(@"city", Required = Required.Always)]
        public string city { get; set; }

        [JsonProperty(@"state_province", Required = Required.Always)]
        public string state_province { get; set; }

        [JsonProperty(@"postal_code", Required = Required.Always)]
        public string postal_code { get; set; }

        [JsonProperty(@"postal_code_extension", NullValueHandling = NullValueHandling.Ignore)]
        public string postal_code_extension { get; set; }

        [JsonProperty(@"county", NullValueHandling = NullValueHandling.Ignore)]
        public string county { get; set; }

        [JsonProperty(@"country", NullValueHandling = NullValueHandling.Ignore)]
        public string country { get; set; }

        [JsonProperty(@"latitude", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? latitude { get; set; }

        [JsonProperty(@"longitude", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? longitude { get; set; }
    }
}
