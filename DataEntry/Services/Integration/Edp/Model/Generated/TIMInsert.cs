using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class TIMInsert : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{request{request_id,action,source_system_name,source_id,source_submitter_name,source_submitter_email,user_role,comment,global_region,country},company_id,area_requested_from,area_requested_from_uom,area_requested_to,area_requested_to_uom,current_space_lease_area,current_space_lease_area_uom,current_space_lease_expiration_date,business_relationship_type_desc,source_create_by,source_create_ts,source_modified_by,source_modified_ts,source_delete_by,source_delete_ts,verified_by,verified_ts,property_usage_type_desc,property_usage_sub_type_desc,party_active_from_date,comment,current_party_location_status_type_desc,status_type_desc,data_acquired_from_desc,reason_space_search_ended_type_desc,current_property_class_desc,target_property_class_desc,renewal_probability_percent,party_target_move_in_date,lease_option_type_desc,record_source_notes,record_source_desc,data_source_notes,party_stopped_looking_for_space_date,target_naics_classification_desc,target_space_cbre_industry_code_desc,party_current_location_notes,party_target_location_notes,reporting_office_id,originated_market_desc,confidentiality_f,verified_f,target_geography{region{region_id,region_desc,market{market_id,market_desc,submarket{submarket_id,submarket_desc,district{district_id,district_desc,neighborhood{neighborhood_id,neighborhood_desc}}}}},lig_geography{id,global_region_id,global_region_desc,division_id,division_desc,market_area_id,market_area_desc,local_market_area_id,local_market_area_desc,market_id,market_desc,submarket_id,submarket_desc,district_id,district_desc,neighborhood_id,neighborhood_desc},city{id,city_desc}},current_geography{region{region_id,region_desc,market{market_id,market_desc,submarket{submarket_id,submarket_desc,district{district_id,district_desc,neighborhood{neighborhood_id,neighborhood_desc}}}}},lig_geography{id,global_region_id,global_region_desc,division_id,division_desc,market_area_id,market_area_desc,local_market_area_id,local_market_area_desc,market_id,market_desc,submarket_id,submarket_desc,district_id,district_desc,neighborhood_id,neighborhood_desc},city{id,city_desc}},company_contact_role_addresses{company_id,contact_id,role_desc,location_id},usages{id,ref_property_usage_type_desc,ref_property_usage_sub_type_desc,ref_property_class_type_desc},user_tag{name},miq_record_hide_f}";

        [JsonProperty(@"request", Required = Required.Always)]
        public RequestDetails request { get; set; }

        [JsonProperty(@"company_id", Required = Required.Always)]
        public int company_id { get; set; }

        [JsonProperty(@"area_requested_from", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? area_requested_from { get; set; }

        [JsonProperty(@"area_requested_from_uom", NullValueHandling = NullValueHandling.Ignore)]
        public string area_requested_from_uom { get; set; }

        [JsonProperty(@"area_requested_to", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? area_requested_to { get; set; }

        [JsonProperty(@"area_requested_to_uom", NullValueHandling = NullValueHandling.Ignore)]
        public string area_requested_to_uom { get; set; }

        [JsonProperty(@"current_space_lease_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? current_space_lease_area { get; set; }

        [JsonProperty(@"current_space_lease_area_uom", NullValueHandling = NullValueHandling.Ignore)]
        public string current_space_lease_area_uom { get; set; }

        [JsonProperty(@"current_space_lease_expiration_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? current_space_lease_expiration_date { get; set; }

        [JsonProperty(@"business_relationship_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string business_relationship_type_desc { get; set; }

        [JsonProperty(@"source_create_by", NullValueHandling = NullValueHandling.Ignore)]
        public string source_create_by { get; set; }

        [JsonProperty(@"source_create_ts", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? source_create_ts { get; set; }

        [JsonProperty(@"source_modified_by", NullValueHandling = NullValueHandling.Ignore)]
        public string source_modified_by { get; set; }

        [JsonProperty(@"source_modified_ts", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? source_modified_ts { get; set; }

        [JsonProperty(@"source_delete_by", NullValueHandling = NullValueHandling.Ignore)]
        public string source_delete_by { get; set; }

        [JsonProperty(@"source_delete_ts", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? source_delete_ts { get; set; }

        [JsonProperty(@"verified_by", NullValueHandling = NullValueHandling.Ignore)]
        public string verified_by { get; set; }

        [JsonProperty(@"verified_ts", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? verified_ts { get; set; }

        ///<summary>
        /// Need to depricate usage fields as MIQ created TIM records can have multi usages
        ///</summary>
        [JsonProperty(@"property_usage_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string property_usage_type_desc { get; set; }

        [JsonProperty(@"property_usage_sub_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string property_usage_sub_type_desc { get; set; }

        [JsonProperty(@"party_active_from_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? party_active_from_date { get; set; }

        [JsonProperty(@"comment", NullValueHandling = NullValueHandling.Ignore)]
        public string comment { get; set; }

        [JsonProperty(@"current_party_location_status_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string current_party_location_status_type_desc { get; set; }

        [JsonProperty(@"status_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string status_type_desc { get; set; }

        [JsonProperty(@"data_acquired_from_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string data_acquired_from_desc { get; set; }

        [JsonProperty(@"reason_space_search_ended_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string reason_space_search_ended_type_desc { get; set; }

        [JsonProperty(@"current_property_class_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string current_property_class_desc { get; set; }

        [JsonProperty(@"target_property_class_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string target_property_class_desc { get; set; }

        [JsonProperty(@"renewal_probability_percent", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? renewal_probability_percent { get; set; }

        [JsonProperty(@"party_target_move_in_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? party_target_move_in_date { get; set; }

        [JsonProperty(@"lease_option_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string lease_option_type_desc { get; set; }

        [JsonProperty(@"record_source_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string record_source_notes { get; set; }

        [JsonProperty(@"record_source_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string record_source_desc { get; set; }

        [JsonProperty(@"data_source_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string data_source_notes { get; set; }

        [JsonProperty(@"party_stopped_looking_for_space_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? party_stopped_looking_for_space_date { get; set; }

        [JsonProperty(@"target_naics_classification_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string target_naics_classification_desc { get; set; }

        [JsonProperty(@"target_space_cbre_industry_code_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string target_space_cbre_industry_code_desc { get; set; }

        [JsonProperty(@"party_current_location_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string party_current_location_notes { get; set; }

        [JsonProperty(@"party_target_location_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string party_target_location_notes { get; set; }

        [JsonProperty(@"reporting_office_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? reporting_office_id { get; set; }

        [JsonProperty(@"originated_market_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string originated_market_desc { get; set; }

        [JsonProperty(@"confidentiality_f", Required = Required.Always)]
        public string confidentiality_f { get; set; }

        [JsonProperty(@"verified_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? verified_f { get; set; }

        [JsonProperty(@"target_geography", Required = Required.Always)]
        public TIMGeography target_geography { get; set; }

        [JsonProperty(@"current_geography", NullValueHandling = NullValueHandling.Ignore)]
        public TIMGeography current_geography { get; set; }

        [JsonProperty(@"company_contact_role_addresses", NullValueHandling = NullValueHandling.Ignore)]
        public List<CompanyContactRoleAddress> company_contact_role_addresses { get; set; }

        [JsonProperty(@"usages", NullValueHandling = NullValueHandling.Ignore)]
        public List<TIMUsages> usages { get; set; }

        [JsonProperty(@"user_tag", NullValueHandling = NullValueHandling.Ignore)]
        public List<UserTag> user_tag { get; set; }

        [JsonProperty(@"miq_record_hide_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? miq_record_hide_f { get; set; }
    }
}
