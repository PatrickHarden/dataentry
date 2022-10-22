using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class BrandInsert : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{request{request_id,action,source_system_name,source_id,source_submitter_name,source_submitter_email,user_role,comment,global_region,country},brand_name,brand_desc,brand_code,origin_country,website,business_category_desc,ref_brand_status_desc,company_contact_role_addresses{company_id,contact_id,role_desc,location_id},brand_media_information{id,media_name,media_path,media_caption,primary_image_f,media_type_desc}}";

        [JsonProperty(@"request", Required = Required.Always)]
        public RequestDetails request { get; set; }

        [JsonProperty(@"brand_name", Required = Required.Always)]
        public string brand_name { get; set; }

        [JsonProperty(@"brand_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string brand_desc { get; set; }

        [JsonProperty(@"brand_code", NullValueHandling = NullValueHandling.Ignore)]
        public string brand_code { get; set; }

        [JsonProperty(@"origin_country", NullValueHandling = NullValueHandling.Ignore)]
        public string origin_country { get; set; }

        [JsonProperty(@"website", NullValueHandling = NullValueHandling.Ignore)]
        public string website { get; set; }

        [JsonProperty(@"business_category_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string business_category_desc { get; set; }

        [JsonProperty(@"ref_brand_status_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_brand_status_desc { get; set; }

        [JsonProperty(@"company_contact_role_addresses", NullValueHandling = NullValueHandling.Ignore)]
        public List<CompanyContactRoleAddress> company_contact_role_addresses { get; set; }

        [JsonProperty(@"brand_media_information", NullValueHandling = NullValueHandling.Ignore)]
        public List<BrandMediaInformation> brand_media_information { get; set; }
    }
}
