using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class Location : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,location_desc,location_long_desc,ref_location_status_code,ref_location_status_desc,headquarter_location_f,agency_location_f,company_location_f,tenant_location_f,billing_location_f,shipping_location_f,postal_address{action,postal_address_id,ref_address_usage_type_desc,street1,street2,address1,pre_street_direction_name,street_name,street_type,post_street_direction_name,city,district,state_province,postal_code,postal_code_extension,suburb_name,county,country,colloquial{id,street1,street2,address1,pre_street_direction_name,street_name,street_type,post_street_direction_name,city,state_province,district,suburb_name,county,country,language_desc,country_code_desc}},phone{local_area_code,phone_number,extension_number,phone_type_desc,usage_type_desc,location_id},lig_geography{id,global_region_id,global_region_desc,division_id,division_desc,market_area_id,market_area_desc,local_market_area_id,local_market_area_desc,market_id,market_desc,submarket_id,submarket_desc,district_id,district_desc,neighborhood_id,neighborhood_desc}}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public int? id { get; set; }

        [JsonProperty(@"location_desc", Required = Required.Always)]
        public string location_desc { get; set; }

        [JsonProperty(@"location_long_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string location_long_desc { get; set; }

        [JsonProperty(@"ref_location_status_code", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_location_status_code { get; set; }

        [JsonProperty(@"ref_location_status_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_location_status_desc { get; set; }

        [JsonProperty(@"headquarter_location_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? headquarter_location_f { get; set; }

        [JsonProperty(@"agency_location_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? agency_location_f { get; set; }

        [JsonProperty(@"company_location_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? company_location_f { get; set; }

        [JsonProperty(@"tenant_location_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? tenant_location_f { get; set; }

        [JsonProperty(@"billing_location_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? billing_location_f { get; set; }

        [JsonProperty(@"shipping_location_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? shipping_location_f { get; set; }

        [JsonProperty(@"postal_address", Required = Required.Always)]
        public PostalAddress postal_address { get; set; }

        [JsonProperty(@"phone", NullValueHandling = NullValueHandling.Ignore)]
        public List<Phone> phone { get; set; }

        [JsonProperty(@"lig_geography", NullValueHandling = NullValueHandling.Ignore)]
        public LIGGeography lig_geography { get; set; }
    }
}
