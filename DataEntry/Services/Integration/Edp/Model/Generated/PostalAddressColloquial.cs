using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class PostalAddressColloquial : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,street1,street2,address1,pre_street_direction_name,street_name,street_type,post_street_direction_name,city,state_province,district,suburb_name,county,country,language_desc,country_code_desc}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public int? id { get; set; }

        [JsonProperty(@"street1", Required = Required.Always)]
        public string street1 { get; set; }

        [JsonProperty(@"street2", NullValueHandling = NullValueHandling.Ignore)]
        public string street2 { get; set; }

        [JsonProperty(@"address1", NullValueHandling = NullValueHandling.Ignore)]
        public string address1 { get; set; }

        [JsonProperty(@"pre_street_direction_name", NullValueHandling = NullValueHandling.Ignore)]
        public string pre_street_direction_name { get; set; }

        [JsonProperty(@"street_name", Required = Required.Always)]
        public string street_name { get; set; }

        [JsonProperty(@"street_type", NullValueHandling = NullValueHandling.Ignore)]
        public string street_type { get; set; }

        [JsonProperty(@"post_street_direction_name", NullValueHandling = NullValueHandling.Ignore)]
        public string post_street_direction_name { get; set; }

        [JsonProperty(@"city", Required = Required.Always)]
        public string city { get; set; }

        [JsonProperty(@"state_province", NullValueHandling = NullValueHandling.Ignore)]
        public string state_province { get; set; }

        [JsonProperty(@"district", NullValueHandling = NullValueHandling.Ignore)]
        public string district { get; set; }

        [JsonProperty(@"suburb_name", NullValueHandling = NullValueHandling.Ignore)]
        public string suburb_name { get; set; }

        [JsonProperty(@"county", NullValueHandling = NullValueHandling.Ignore)]
        public string county { get; set; }

        [JsonProperty(@"country", NullValueHandling = NullValueHandling.Ignore)]
        public string country { get; set; }

        [JsonProperty(@"language_desc", Required = Required.Always)]
        public string language_desc { get; set; }

        [JsonProperty(@"country_code_desc", Required = Required.Always)]
        public string country_code_desc { get; set; }
    }
}
