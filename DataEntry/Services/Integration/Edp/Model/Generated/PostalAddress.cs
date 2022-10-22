using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class PostalAddress : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{action,postal_address_id,ref_address_usage_type_desc,street1,street2,address1,pre_street_direction_name,street_name,street_type,post_street_direction_name,city,district,state_province,postal_code,postal_code_extension,suburb_name,county,country,colloquial{id,street1,street2,address1,pre_street_direction_name,street_name,street_type,post_street_direction_name,city,state_province,district,suburb_name,county,country,language_desc,country_code_desc}}";

        [JsonProperty(@"action", NullValueHandling = NullValueHandling.Ignore)]
        public string action { get; set; }

        [JsonProperty(@"postal_address_id", NullValueHandling = NullValueHandling.Ignore)]
        public string postal_address_id { get; set; }

        [JsonProperty(@"ref_address_usage_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_address_usage_type_desc { get; set; }

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

        [JsonProperty(@"district", NullValueHandling = NullValueHandling.Ignore)]
        public string district { get; set; }

        [JsonProperty(@"state_province", NullValueHandling = NullValueHandling.Ignore)]
        public string state_province { get; set; }

        [JsonProperty(@"postal_code", NullValueHandling = NullValueHandling.Ignore)]
        public string postal_code { get; set; }

        [JsonProperty(@"postal_code_extension", NullValueHandling = NullValueHandling.Ignore)]
        public string postal_code_extension { get; set; }

        [JsonProperty(@"suburb_name", NullValueHandling = NullValueHandling.Ignore)]
        public string suburb_name { get; set; }

        [JsonProperty(@"county", NullValueHandling = NullValueHandling.Ignore)]
        public string county { get; set; }

        [JsonProperty(@"country", Required = Required.Always)]
        public string country { get; set; }

        [JsonProperty(@"colloquial", NullValueHandling = NullValueHandling.Ignore)]
        public List<PostalAddressColloquial> colloquial { get; set; }
    }
}
