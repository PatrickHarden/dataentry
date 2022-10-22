using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class CompanyUnified : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,name,postal_address{id,street1,street2,street_name,pre_street_direction_name,street_type,post_street_direction_name,address_line_1,city,state_province,postal_code,postal_code_extension,county,country,latitude,longitude},contact{id,name,email,phone},website,phone}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public string id { get; set; }

        [JsonProperty(@"name", Required = Required.Always)]
        public string name { get; set; }

        [JsonProperty(@"postal_address", Required = Required.Always)]
        public PostalAddressUnified postal_address { get; set; }

        [JsonProperty(@"contact", NullValueHandling = NullValueHandling.Ignore)]
        public List<ContactUnified> contact { get; set; }

        [JsonProperty(@"website", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> website { get; set; }

        [JsonProperty(@"phone", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> phone { get; set; }
    }
}
