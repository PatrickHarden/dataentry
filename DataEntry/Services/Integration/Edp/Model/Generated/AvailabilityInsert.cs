using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class AvailabilityInsert : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{request{request_id,action,source_system_name,source_id,source_submitter_name,source_submitter_email,user_role,comment,global_region,country},date_available,date_on_market,statistical_listing_f,listing_descr,listing_expiration_date,listing_notes,listing_source,modified_on,researcher_notes,status_change_date,vacant_f,amperage,asking_rental_rate_monthly,asking_rental_rate_yearly,asking_rental_rate_monthly_uom_desc,asking_rental_rate_yearly_uom_desc,asking_rental_rate_yearly_area_uom_desc,available_office_space,available_office_space_uom_desc,bay_count,bay_depth,bay_width,ceiling_height,clear_height,clear_height_uom_desc,maximum_clear_height,maximum_clear_height_uom_desc,minimum_clear_height,minimum_clear_height_uom_desc,minimum_divisible_surface_area,minimum_divisible_surface_area_uom_desc,number_of_dock_doors,number_of_grade_level_door,number_of_rail_door,electric_type_id,number_of_month_free_rent,frontage_descr,cooking_area_f,dock_door_f,grade_level_iron_door_f,door_rail_f,yard_space_available_f,inclusion_amount,inclusion_amount_uom_desc,corner_space_f,food_permitted_f,side_of_street_f,vent_available_f,lease_comp_arrangement_id,lease_status_by,lease_status_date,maximum_lease_term,minumum_lease_term,modified_by,parking_cost,parking_cost_uom_desc,parking_cost_notes,percentage_rent,ref_rail_service_type_desc,available_space,available_space_uom_desc,sub_divide_allowed_f,sub_lease_expire_date,tenent_improvement_allowance,tenent_improvement_allowance_uom_desc,tenent_improvement_allowance_text,total_area_of_space,total_area_of_space_uom_desc,voltage,asking_price_for_sale,asking_price_for_sale_uom_desc,sale_comment,sale_comp_arrangement_id,ref_availability_type_desc,company_contact_role_addresses{company_id,contact_id,role_desc,location_id},total_contiguous_area_of_space,total_contiguous_area_of_space_uom_desc,ref_possession_type_desc,ref_space_condition_type_desc,grade_level_door_f,ref_space_availability_status_desc,ref_off_market_reason_desc,retail_address_notes,user_configured_contiguous_space_f,ref_retail_location_type_desc,contiguous_space_number,ref_lease_type_desc,ref_sale_type_desc,ref_rate_type_desc,minimum_asking_rate_monthly,minimum_asking_rate_monthly_uom,maximum_asking_rate_monthly,maximum_asking_rate_monthly_uom,minimum_asking_rate_yearly,minimum_asking_rate_yearly_uom,maximum_asking_rate_yearly,maximum_asking_rate_yearly_uom,property_id,property_usage_id,full_floor_f,floor_suite{floor_number,suite_number},user_tag{name},security_deposit_amount,security_deposit_amount_uom_desc,record_source_desc,data_source_notes,data_acquired_from_desc,record_source_notes,property_measurement_availability{id,property_element_type_desc,property_measurement_notes,property_measurement_size,property_measurement_size_uom},miq_record_hide_f,carpet_area,carpet_area_uom_desc,common_area,common_area_uom_desc,cam_charges,cam_charges_uom_desc,listing_ids{listing_id,listing_source},security_deposit_value,security_deposit_value_uom_desc}";

        [JsonProperty(@"request", Required = Required.Always)]
        public RequestDetails request { get; set; }

        [JsonProperty(@"date_available", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? date_available { get; set; }

        [JsonProperty(@"date_on_market", Required = Required.Always)]
        public DateTime date_on_market { get; set; }

        [JsonProperty(@"statistical_listing_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? statistical_listing_f { get; set; }

        [JsonProperty(@"listing_descr", NullValueHandling = NullValueHandling.Ignore)]
        public string listing_descr { get; set; }

        [JsonProperty(@"listing_expiration_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? listing_expiration_date { get; set; }

        [JsonProperty(@"listing_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string listing_notes { get; set; }

        [JsonProperty(@"listing_source", NullValueHandling = NullValueHandling.Ignore)]
        public string listing_source { get; set; }

        [JsonProperty(@"modified_on", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? modified_on { get; set; }

        [JsonProperty(@"researcher_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string researcher_notes { get; set; }

        [JsonProperty(@"status_change_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? status_change_date { get; set; }

        [JsonProperty(@"vacant_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? vacant_f { get; set; }

        [JsonProperty(@"amperage", NullValueHandling = NullValueHandling.Ignore)]
        public string amperage { get; set; }

        [JsonProperty(@"asking_rental_rate_monthly", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? asking_rental_rate_monthly { get; set; }

        [JsonProperty(@"asking_rental_rate_yearly", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? asking_rental_rate_yearly { get; set; }

        [JsonProperty(@"asking_rental_rate_monthly_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string asking_rental_rate_monthly_uom_desc { get; set; }

        [JsonProperty(@"asking_rental_rate_yearly_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string asking_rental_rate_yearly_uom_desc { get; set; }

        [JsonProperty(@"asking_rental_rate_yearly_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string asking_rental_rate_yearly_area_uom_desc { get; set; }

        [JsonProperty(@"available_office_space", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? available_office_space { get; set; }

        [JsonProperty(@"available_office_space_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string available_office_space_uom_desc { get; set; }

        [JsonProperty(@"bay_count", NullValueHandling = NullValueHandling.Ignore)]
        public int? bay_count { get; set; }

        [JsonProperty(@"bay_depth", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? bay_depth { get; set; }

        [JsonProperty(@"bay_width", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? bay_width { get; set; }

        [JsonProperty(@"ceiling_height", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ceiling_height { get; set; }

        [JsonProperty(@"clear_height", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? clear_height { get; set; }

        [JsonProperty(@"clear_height_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string clear_height_uom_desc { get; set; }

        [JsonProperty(@"maximum_clear_height", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? maximum_clear_height { get; set; }

        [JsonProperty(@"maximum_clear_height_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string maximum_clear_height_uom_desc { get; set; }

        [JsonProperty(@"minimum_clear_height", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? minimum_clear_height { get; set; }

        [JsonProperty(@"minimum_clear_height_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string minimum_clear_height_uom_desc { get; set; }

        [JsonProperty(@"minimum_divisible_surface_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? minimum_divisible_surface_area { get; set; }

        [JsonProperty(@"minimum_divisible_surface_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string minimum_divisible_surface_area_uom_desc { get; set; }

        [JsonProperty(@"number_of_dock_doors", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? number_of_dock_doors { get; set; }

        [JsonProperty(@"number_of_grade_level_door", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? number_of_grade_level_door { get; set; }

        [JsonProperty(@"number_of_rail_door", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? number_of_rail_door { get; set; }

        [JsonProperty(@"electric_type_id", NullValueHandling = NullValueHandling.Ignore)]
        public string electric_type_id { get; set; }

        [JsonProperty(@"number_of_month_free_rent", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? number_of_month_free_rent { get; set; }

        [JsonProperty(@"frontage_descr", NullValueHandling = NullValueHandling.Ignore)]
        public string frontage_descr { get; set; }

        [JsonProperty(@"cooking_area_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? cooking_area_f { get; set; }

        [JsonProperty(@"dock_door_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? dock_door_f { get; set; }

        [JsonProperty(@"grade_level_iron_door_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? grade_level_iron_door_f { get; set; }

        [JsonProperty(@"door_rail_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? door_rail_f { get; set; }

        [JsonProperty(@"yard_space_available_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? yard_space_available_f { get; set; }

        [JsonProperty(@"inclusion_amount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? inclusion_amount { get; set; }

        [JsonProperty(@"inclusion_amount_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string inclusion_amount_uom_desc { get; set; }

        [JsonProperty(@"corner_space_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? corner_space_f { get; set; }

        [JsonProperty(@"food_permitted_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? food_permitted_f { get; set; }

        [JsonProperty(@"side_of_street_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? side_of_street_f { get; set; }

        [JsonProperty(@"vent_available_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? vent_available_f { get; set; }

        [JsonProperty(@"lease_comp_arrangement_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? lease_comp_arrangement_id { get; set; }

        [JsonProperty(@"lease_status_by", NullValueHandling = NullValueHandling.Ignore)]
        public string lease_status_by { get; set; }

        [JsonProperty(@"lease_status_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? lease_status_date { get; set; }

        [JsonProperty(@"maximum_lease_term", NullValueHandling = NullValueHandling.Ignore)]
        public string maximum_lease_term { get; set; }

        [JsonProperty(@"minumum_lease_term", NullValueHandling = NullValueHandling.Ignore)]
        public string minumum_lease_term { get; set; }

        [JsonProperty(@"modified_by", NullValueHandling = NullValueHandling.Ignore)]
        public string modified_by { get; set; }

        [JsonProperty(@"parking_cost", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? parking_cost { get; set; }

        [JsonProperty(@"parking_cost_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string parking_cost_uom_desc { get; set; }

        [JsonProperty(@"parking_cost_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string parking_cost_notes { get; set; }

        [JsonProperty(@"percentage_rent", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? percentage_rent { get; set; }

        [JsonProperty(@"ref_rail_service_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_rail_service_type_desc { get; set; }

        [JsonProperty(@"available_space", Required = Required.Always)]
        public decimal available_space { get; set; }

        [JsonProperty(@"available_space_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string available_space_uom_desc { get; set; }

        [JsonProperty(@"sub_divide_allowed_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? sub_divide_allowed_f { get; set; }

        [JsonProperty(@"sub_lease_expire_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? sub_lease_expire_date { get; set; }

        [JsonProperty(@"tenent_improvement_allowance", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? tenent_improvement_allowance { get; set; }

        [JsonProperty(@"tenent_improvement_allowance_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string tenent_improvement_allowance_uom_desc { get; set; }

        [JsonProperty(@"tenent_improvement_allowance_text", NullValueHandling = NullValueHandling.Ignore)]
        public string tenent_improvement_allowance_text { get; set; }

        [JsonProperty(@"total_area_of_space", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? total_area_of_space { get; set; }

        [JsonProperty(@"total_area_of_space_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string total_area_of_space_uom_desc { get; set; }

        [JsonProperty(@"voltage", NullValueHandling = NullValueHandling.Ignore)]
        public string voltage { get; set; }

        [JsonProperty(@"asking_price_for_sale", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? asking_price_for_sale { get; set; }

        [JsonProperty(@"asking_price_for_sale_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string asking_price_for_sale_uom_desc { get; set; }

        [JsonProperty(@"sale_comment", NullValueHandling = NullValueHandling.Ignore)]
        public string sale_comment { get; set; }

        [JsonProperty(@"sale_comp_arrangement_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? sale_comp_arrangement_id { get; set; }

        [JsonProperty(@"ref_availability_type_desc", Required = Required.Always)]
        public string ref_availability_type_desc { get; set; }

        [JsonProperty(@"company_contact_role_addresses", Required = Required.Always)]
        public List<CompanyContactRoleAddress> company_contact_role_addresses { get; set; }

        [JsonProperty(@"total_contiguous_area_of_space", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? total_contiguous_area_of_space { get; set; }

        [JsonProperty(@"total_contiguous_area_of_space_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string total_contiguous_area_of_space_uom_desc { get; set; }

        [JsonProperty(@"ref_possession_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_possession_type_desc { get; set; }

        [JsonProperty(@"ref_space_condition_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_space_condition_type_desc { get; set; }

        [JsonProperty(@"grade_level_door_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? grade_level_door_f { get; set; }

        [JsonProperty(@"ref_space_availability_status_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_space_availability_status_desc { get; set; }

        [JsonProperty(@"ref_off_market_reason_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_off_market_reason_desc { get; set; }

        [JsonProperty(@"retail_address_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string retail_address_notes { get; set; }

        [JsonProperty(@"user_configured_contiguous_space_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? user_configured_contiguous_space_f { get; set; }

        [JsonProperty(@"ref_retail_location_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_retail_location_type_desc { get; set; }

        [JsonProperty(@"contiguous_space_number", NullValueHandling = NullValueHandling.Ignore)]
        public int? contiguous_space_number { get; set; }

        [JsonProperty(@"ref_lease_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_lease_type_desc { get; set; }

        [JsonProperty(@"ref_sale_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_sale_type_desc { get; set; }

        [JsonProperty(@"ref_rate_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_rate_type_desc { get; set; }

        [JsonProperty(@"minimum_asking_rate_monthly", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? minimum_asking_rate_monthly { get; set; }

        [JsonProperty(@"minimum_asking_rate_monthly_uom", NullValueHandling = NullValueHandling.Ignore)]
        public string minimum_asking_rate_monthly_uom { get; set; }

        [JsonProperty(@"maximum_asking_rate_monthly", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? maximum_asking_rate_monthly { get; set; }

        [JsonProperty(@"maximum_asking_rate_monthly_uom", NullValueHandling = NullValueHandling.Ignore)]
        public string maximum_asking_rate_monthly_uom { get; set; }

        [JsonProperty(@"minimum_asking_rate_yearly", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? minimum_asking_rate_yearly { get; set; }

        [JsonProperty(@"minimum_asking_rate_yearly_uom", NullValueHandling = NullValueHandling.Ignore)]
        public string minimum_asking_rate_yearly_uom { get; set; }

        [JsonProperty(@"maximum_asking_rate_yearly", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? maximum_asking_rate_yearly { get; set; }

        [JsonProperty(@"maximum_asking_rate_yearly_uom", NullValueHandling = NullValueHandling.Ignore)]
        public string maximum_asking_rate_yearly_uom { get; set; }

        [JsonProperty(@"property_id", Required = Required.Always)]
        public int property_id { get; set; }

        [JsonProperty(@"property_usage_id", Required = Required.Always)]
        public int property_usage_id { get; set; }

        [JsonProperty(@"full_floor_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? full_floor_f { get; set; }

        [JsonProperty(@"floor_suite", NullValueHandling = NullValueHandling.Ignore)]
        public FloorNSuite floor_suite { get; set; }

        [JsonProperty(@"user_tag", NullValueHandling = NullValueHandling.Ignore)]
        public List<UserTag> user_tag { get; set; }

        [JsonProperty(@"security_deposit_amount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? security_deposit_amount { get; set; }

        [JsonProperty(@"security_deposit_amount_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string security_deposit_amount_uom_desc { get; set; }

        [JsonProperty(@"record_source_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string record_source_desc { get; set; }

        [JsonProperty(@"data_source_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string data_source_notes { get; set; }

        [JsonProperty(@"data_acquired_from_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string data_acquired_from_desc { get; set; }

        [JsonProperty(@"record_source_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string record_source_notes { get; set; }

        [JsonProperty(@"property_measurement_availability", NullValueHandling = NullValueHandling.Ignore)]
        public List<PropertyMeasurement> property_measurement_availability { get; set; }

        [JsonProperty(@"miq_record_hide_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? miq_record_hide_f { get; set; }

        [JsonProperty(@"carpet_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? carpet_area { get; set; }

        [JsonProperty(@"carpet_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string carpet_area_uom_desc { get; set; }

        [JsonProperty(@"common_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? common_area { get; set; }

        [JsonProperty(@"common_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string common_area_uom_desc { get; set; }

        [JsonProperty(@"cam_charges", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? cam_charges { get; set; }

        [JsonProperty(@"cam_charges_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string cam_charges_uom_desc { get; set; }

        [JsonProperty(@"listing_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<ListingID> listing_ids { get; set; }

        [JsonProperty(@"security_deposit_value", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? security_deposit_value { get; set; }

        [JsonProperty(@"security_deposit_value_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string security_deposit_value_uom_desc { get; set; }
    }
}
