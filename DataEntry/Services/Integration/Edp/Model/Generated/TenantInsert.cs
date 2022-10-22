using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class TenantInsert : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{request{request_id,action,source_system_name,source_id,source_submitter_name,source_submitter_email,user_role,comment,global_region,country},company_id,property_id,ref_naics_classification_desc,tenant_cbre_industry_code_desc,confidentiality_f,total_employee_at_current_location,source_create_by,source_create_ts,source_modified_by,source_modified_ts,record_source_desc,total_area,total_area_uom_desc,data_source_notes,data_acquired_from_desc,record_source_notes,tenancy_notes,property_space{floor_number,suite_number,sublease_f,leased_space_size,leased_space_size_uom_desc,lease_start_date,lease_expiration_date,headquarter_location_f,tenancy_status,lease_id,occupancy_date,ref_property_usage_type_desc,ref_property_usage_sub_type_desc,brand_id,estimated_rent_amount,estimated_rent_amount_uom,external_opportunity_number,external_opportunity_number_source_desc},user_tag{name},miq_record_hide_f}";

        [JsonProperty(@"request", Required = Required.Always)]
        public RequestDetails request { get; set; }

        [JsonProperty(@"company_id", Required = Required.Always)]
        public int company_id { get; set; }

        [JsonProperty(@"property_id", Required = Required.Always)]
        public int property_id { get; set; }

        [JsonProperty(@"ref_naics_classification_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_naics_classification_desc { get; set; }

        [JsonProperty(@"tenant_cbre_industry_code_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string tenant_cbre_industry_code_desc { get; set; }

        [JsonProperty(@"confidentiality_f", NullValueHandling = NullValueHandling.Ignore)]
        public string confidentiality_f { get; set; }

        [JsonProperty(@"total_employee_at_current_location", NullValueHandling = NullValueHandling.Ignore)]
        public int? total_employee_at_current_location { get; set; }

        [JsonProperty(@"source_create_by", NullValueHandling = NullValueHandling.Ignore)]
        public string source_create_by { get; set; }

        [JsonProperty(@"source_create_ts", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? source_create_ts { get; set; }

        [JsonProperty(@"source_modified_by", NullValueHandling = NullValueHandling.Ignore)]
        public string source_modified_by { get; set; }

        [JsonProperty(@"source_modified_ts", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? source_modified_ts { get; set; }

        [JsonProperty(@"record_source_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string record_source_desc { get; set; }

        [JsonProperty(@"total_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? total_area { get; set; }

        [JsonProperty(@"total_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string total_area_uom_desc { get; set; }

        [JsonProperty(@"data_source_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string data_source_notes { get; set; }

        [JsonProperty(@"data_acquired_from_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string data_acquired_from_desc { get; set; }

        [JsonProperty(@"record_source_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string record_source_notes { get; set; }

        [JsonProperty(@"tenancy_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string tenancy_notes { get; set; }

        [JsonProperty(@"property_space", Required = Required.Always)]
        public List<TenantPropertyDetail> property_space { get; set; }

        [JsonProperty(@"user_tag", NullValueHandling = NullValueHandling.Ignore)]
        public List<UserTag> user_tag { get; set; }

        [JsonProperty(@"miq_record_hide_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? miq_record_hide_f { get; set; }
    }
}
