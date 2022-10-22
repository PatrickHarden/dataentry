using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class PropertyInsert : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{request{request_id,action,source_system_name,source_id,source_submitter_name,source_submitter_email,user_role,comment,global_region,country},property_name,type_desc,total_land_area,total_land_area_uom_desc,property_descr,property_zoning_code,property_notes,last_sale_price,last_sale_price_uom_desc,last_purchase_date,owner_occupied_f,total_gross_area,total_gross_area_uom_desc,net_rentable_area,net_rentable_area_uom_desc,ref_property_status_desc,year_property_built,month_property_built,statistical_f,cbre_owned_property_f,number_parking_space,parking_ratio,operating_expense,operating_expense_uom_desc,operating_expense_year,ref_property_construction_type_desc,number_elevator_bank,number_of_passanger_elevator,passanger_elevator_capacity,passanger_elevator_capacity_uom_desc,number_freight_elevator,freight_elevator_capacity,freight_elevator_capacity_uom_desc,cam_charge,ref_cam_charge_uom_desc,iron_mountain_file_f,block_and_tract_number,lot_number,minimum_floor_area,minimum_floor_area_uom_desc,maximum_floor_area,maximum_floor_area_uom_desc,typical_floor_area,typical_floor_area_uom_desc,ref_property_sprinkler_type_desc,terrace_area,terrace_area_uom_desc,premier_property_f,ref_built_to_specification_desc,ref_data_acquired_from_desc,floor_area_ratio,loss_factor,lot_coverage,ref_possession_type_desc,total_number_of_floors,ref_property_tenancy_type_desc,property_plant_and_machinery_f,building_common_name,month_property_renovated,year_property_renovated,cbre_managed_property_f,occupancy_rate,prime_property_f,property_class_type_desc,property_office_park_name,cam_charge_note,record_source_desc,dig_notes,parking_comment,record_source_notes,source_notes,year_construction_started,month_construction_started,floor_loading_capacity,floor_loading_capacity_uom,property_status_change_date,floor_loading_capacity_uom_desc,associated_project_completion_date,number_of_floor_below_ground,number_of_floor_above_ground,power_allocation_per_floor,power_allocation_per_floor_uom,property_ownership_type_desc,power_allocation_for_building,power_allocation_for_building_uom,usages{id,ref_property_usage_type_desc,ref_property_usage_sub_type_desc,cbre_managed_direct_surface_area_available,cbre_managed_direct_surface_area_available_uom_desc,cbre_managed_sub_lease_surface_area_available,cbre_managed_sub_lease_surface_area_available_uom_desc,date_property_available,direct_vacant_area,direct_vacant_area_uom_desc,estimated_maximum_monthly_direct_lease_rate,estimated_maximum_monthly_direct_lease_rate_uom_desc,estimated_maximum_yearly_direct_lease_rate,estimated_maximum_yearly_direct_lease_rate_uom_desc,estimated_minimum_monthly_direct_lease_rate,estimated_minimum_monthly_direct_lease_rate_uom_desc,estimated_minimum_yearly_direct_lease_rate,estimated_minimum_yearly_direct_lease_rate_uom_desc,full_floor_available_f,maximum_contiguous_surface_area,maximum_contiguous_surface_area_uom_desc,maximum_lease_term,maximum_lease_term_uom_desc,maximum_monthly_direct_lease_rate,maximum_monthly_direct_lease_rate_uom_desc,maximum_yearly_direct_lease_rate,maximum_yearly_direct_lease_rate_uom_desc,minimum_divisible_surface,minimum_divisible_surface_area_uom_desc,minimum_lease_term,minimum_lease_term_uom_desc,minimum_monthly_direct_lease_rate,minimum_monthly_direct_lease_rate_uom_desc,minimum_monthly_sub_lease_rate,minimum_monthly_sub_lease_rate_uom_desc,total_available_area_sub_lease,total_available_area_sub_lease_uom_desc,total_available_surface_area,total_available_surface_area_uom_desc,total_direct_available_surface_area,total_direct_available_surface_area_uom_desc,total_vacant_area,total_vacant_area_uom_desc,net_rentable_area,net_rentable_area_uom_desc,total_gross_area,total_gross_area_uom_desc,ref_property_class_type_desc,primary_usage_f,ref_sublease_rate_type_desc,column_spacing_uom_desc,operating_expense_amount,operating_expense_amount_uom_desc,prime_property_f,yard_space_available_f,ref_direct_rate_type_desc,maximum_monthly_sub_lease_rate,maximum_monthly_sub_lease_rate_uom_desc,maximum_yearly_sub_lease_rate,maximum_yearly_sub_lease_rate_uom_desc,tenant_improvement_allowance_amount,tenant_improvement_allowance_amount_uom_desc,office_area,office_area_uom_desc,bay_size_length,bay_size_length_uom_desc,bay_size_width,bay_size_width_uom_desc,maximum_clear_height,maximum_clear_height_uom_desc,minimum_clear_height,minimum_clear_height_uom_desc,number_available_crane,crane_height,crane_height_uom_desc,crane_tonnage,crane_tonnage_uom_desc,crane_served_f,cross_dock_position_exist_f,number_cross_dock_position,door_rail_exist_f,number_doos_rail,excess_land_exists_f,exterior_position_exist_f,exterior_position_number,available_warehouse_area,available_warehouse_area_uom_desc,available_freezer_area,available_freezer_area_uom_desc,warehouse_air_conditioner_area,warehouse_air_conditioner_area_uom_desc,clean_room_area,clean_room_area_uom_desc,refrigerator_area,refrigerator_area_uom_desc,computer_area,computer_area_uom_desc,interior_position_exist_f,number_interior_position,number_bays,ref_rail_service_type_desc,truckwell_position_exist_f,number_truckwell_position,road_corridor,number_dock_door,dockdoor_exist_f,grade_doors_dock_number,grade_dock_door_exist_f,number_tenants,number_anchor_tenants,anchor_note,available_frontage_area,available_frontage_area_uom_desc,pads_available_f,voltage,amperage,depth_length,depth_length_uom_desc,flood_map_number,flood_plain,frontage_length,frontage_length_uom_desc,gas_type_desc,has_broadband_f,has_electrical_f,has_in_ground_tanks_f,has_telephone_f,heat_type_desc,institutional_f,insurance_cost,insurance_cost_uom_desc,lighting_system_desc,maintenence_cost,maintenence_cost_uom_desc,major_street_frontage,marketing_statistical_area,minimum_available_area,minimum_available_area_uom_desc,minimum_yearly_direct_lease_rate_amount,minimum_yearly_direct_lease_rate_amount_uom_desc,minimum_yearly_sub_lease_rate,minimum_yearly_sub_lease_rate_uom_desc,number_parcel,other_expense_amount,other_expense_amount_uom_desc,potential_uses,potential_zoning_code,price_per_unit_value,price_per_unit_value_uom_desc,primary_access,sewer_type_desc,stop_expense_amount,stop_expense_amount_uom_desc,sub_lease_vacant_area,sub_lease_vacant_area_uom_desc,surrounding_uses,topography_desc,total_area_not_available_vacant,total_area_not_available_vacant_uom_desc,direct_vacant_not_available_area,direct_vacant_not_available_area_uom_desc,sublease_vacant_not_available_area,sublease_vacant_not_available_area_uom_desc,total_number_of_unit,total_number_of_unit_uom_desc,total_expense,total_expense_uom_desc,units_per_area_value,units_per_area_value_uom_desc,utilities_expense_amount,utilities_expense_amount_uom_desc,vacancy_index_desc,source_verified_f,water_type_desc,available_land_area,available_land_area_uom_desc,corner_information,column_spacing,parking_type_desc,has_drive_through_dock_f,shared_access_f,bay_size_height,bay_size_height_uom_desc,additional_availability_notes,availability_notes,roof_comment,rail_provider_note,crane_note,hvac_comment,statistical_f,average_monthly_direct_lease_rate,average_monthly_direct_lease_rate_uom,average_monthly_sub_lease_rate,average_monthly_sub_lease_rate_uom,average_yearly_direct_lease_rate,average_yearly_direct_lease_rate_uom,average_yearly_sub_lease_rate,average_yearly_sub_lease_rate_uom,tenant_improvement_as_is_f,parking_expense_amount,parking_expense_amount_uom_desc,tax_expense_amount,tax_expense_amount_uom_desc,tax_expense_year,additional_property_usage_note,geography{global_region_desc,region_desc,market_desc,submarket_desc,district_desc,neighborhood_desc},lig_geography{id,global_region_id,global_region_desc,division_id,division_desc,market_area_id,market_area_desc,local_market_area_id,local_market_area_desc,market_id,market_desc,submarket_id,submarket_desc,district_id,district_desc,neighborhood_id,neighborhood_desc},usage_amenity{id,amenity_name,property_amenity_type_desc,amenity_desc,amenity_notes,amenity_distance_from_property,amenity_distance_from_property_uom,amenity_purchase_price,amenity_purchase_price_uom,size,colloquial{id,amenity_name,size,country_code_desc,language_desc,amenity_desc,amenity_notes}},property_parking{id,parking_comments,parking_type_desc,parking_sub_type_desc,parking_cost_monthly,parking_cost_monthly_uom,parking_cost_hourly,parking_cost_hourly_uom,parking_cost_hourly_start,parking_cost_hourly_start_uom,parking_cost_hourly_end,parking_cost_hourly_end_uom,parking_cost_monthly_start,parking_cost_monthly_start_uom,parking_cost_monthly_end,parking_cost_monthly_end_uom},company_contact_role_addresses{company_id,contact_id,role_desc,location_id},number_parking_space,colloquial{id,additional_property_usage_note,country_code_desc,language_desc},operating_expense_amount_yearly_f},postal_address{action,postal_address_id,ref_address_usage_type_desc,street1,street2,address1,pre_street_direction_name,street_name,street_type,post_street_direction_name,city,district,state_province,postal_code,postal_code_extension,suburb_name,county,country,colloquial{id,street1,street2,address1,pre_street_direction_name,street_name,street_type,post_street_direction_name,city,state_province,district,suburb_name,county,country,language_desc,country_code_desc}},elevator_bank{id,elevator_bank_name,total_number_of_elevators,start_floor,end_floor,elevator_bank_notes,colloquial{id,elevator_bank_name,elevator_bank_notes,country_code_desc,language_desc}},property_note{id,property_notes_type_desc,notes,colloquial{id,notes,country_code_desc,language_desc}},digital{digital_address,digital_type_desc,usage_type_desc,digital_address_display_text},property_media_information{id,media_name,media_path,primary_image_f,ref_property_image_type_desc,still_video_f,ref_image_size_type_desc,ref_media_source_system_desc,publish_image_f,image_cbre_owned_f,media_caption,image_width,image_height,ref_image_orientation_desc,thumbnail_f,loopnet_image_f,allow_web_view_f,photographer_party_skey,source_notes,ref_media_quality_type_desc,ref_media_view_type_desc,ref_property_usage_type_desc,watermark_label,watermark_precision_probability,media_type_desc,media_content_type_desc,display_order},property_external_rating{id,green_building_rating_type_desc,green_building_category_desc,green_building_cert_level_desc,green_building_cert_date,year_certified},property_external_ratings{id,green_building_rating_type_desc,green_building_category_desc,green_building_cert_level_desc,green_building_cert_date,year_certified},property_apn_number{id,property_apn_number},property_amenity{id,amenity_name,property_amenity_type_desc,amenity_desc,amenity_notes,amenity_distance_from_property,amenity_distance_from_property_uom,amenity_purchase_price,amenity_purchase_price_uom,size,colloquial{id,amenity_name,size,country_code_desc,language_desc,amenity_desc,amenity_notes}},property_tax_assessment{id,tax_assessment_year,tax_amount,tax_amount_uom_desc,assessed_value_for_improvement,assessed_value_for_improvement_uom_desc,assessed_value_for_land,assessed_value_for_land_uom_desc,total_assessed_value_for_property,total_assessed_value_for_property_uom_desc},asset{id,asset_name,asset_type_desc,ref_asset_sub_type_code,asset_sub_type_desc,asset_desc,asset_capacity,asset_capacity_uom,colloquial{id,asset_name,asset_desc,country_code_desc,language_desc}},property_elevator{id,elevator_type_desc,number_of_elevator,elevator_capacity,elevator_capacity_uom,elevator_notes,elevator_presence_flag},property_air_condition{id,property_air_condition_desc,air_condition_type_desc,property_air_condition_note,week_day_type_desc,start_time,end_time},colloquial{id,property_name,property_desc,property_type_desc,parking_comment,country_code_desc,language_desc},space_to_office_conversion_efficiency_rate,space_utilization_efficiency_rate,user_tag{name},property_landlord_provision{id,property_landlord_provision_desc,landlord_provision_type_notes,landlord_provision_type_desc,colloquial{id,property_landlord_provision_note,country_code_desc,language_desc}},property_measurement{id,property_element_type_desc,property_measurement_notes,property_measurement_size,property_measurement_size_uom},property_frontage{id,frontage_length,frontage_length_uom_desc,frontage_street_name,frontage_street_type},source_details,miq_record_hide_f}";

        [JsonProperty(@"request", Required = Required.Always)]
        public RequestDetails request { get; set; }

        [JsonProperty(@"property_name", Required = Required.Always)]
        public string property_name { get; set; }

        [JsonProperty(@"type_desc", Required = Required.Always)]
        public string type_desc { get; set; }

        [JsonProperty(@"total_land_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? total_land_area { get; set; }

        [JsonProperty(@"total_land_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string total_land_area_uom_desc { get; set; }

        [JsonProperty(@"property_descr", NullValueHandling = NullValueHandling.Ignore)]
        public string property_descr { get; set; }

        [JsonProperty(@"property_zoning_code", NullValueHandling = NullValueHandling.Ignore)]
        public string property_zoning_code { get; set; }

        [JsonProperty(@"property_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string property_notes { get; set; }

        [JsonProperty(@"last_sale_price", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? last_sale_price { get; set; }

        [JsonProperty(@"last_sale_price_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string last_sale_price_uom_desc { get; set; }

        [JsonProperty(@"last_purchase_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? last_purchase_date { get; set; }

        [JsonProperty(@"owner_occupied_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? owner_occupied_f { get; set; }

        [JsonProperty(@"total_gross_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? total_gross_area { get; set; }

        [JsonProperty(@"total_gross_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string total_gross_area_uom_desc { get; set; }

        [JsonProperty(@"net_rentable_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? net_rentable_area { get; set; }

        [JsonProperty(@"net_rentable_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string net_rentable_area_uom_desc { get; set; }

        [JsonProperty(@"ref_property_status_desc", Required = Required.Always)]
        public string ref_property_status_desc { get; set; }

        [JsonProperty(@"year_property_built", NullValueHandling = NullValueHandling.Ignore)]
        public int? year_property_built { get; set; }

        [JsonProperty(@"month_property_built", NullValueHandling = NullValueHandling.Ignore)]
        public int? month_property_built { get; set; }

        [JsonProperty(@"statistical_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? statistical_f { get; set; }

        [JsonProperty(@"cbre_owned_property_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? cbre_owned_property_f { get; set; }

        [JsonProperty(@"number_parking_space", NullValueHandling = NullValueHandling.Ignore)]
        public int? number_parking_space { get; set; }

        [JsonProperty(@"parking_ratio", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? parking_ratio { get; set; }

        [JsonProperty(@"operating_expense", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? operating_expense { get; set; }

        [JsonProperty(@"operating_expense_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string operating_expense_uom_desc { get; set; }

        [JsonProperty(@"operating_expense_year", NullValueHandling = NullValueHandling.Ignore)]
        public int? operating_expense_year { get; set; }

        [JsonProperty(@"ref_property_construction_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_property_construction_type_desc { get; set; }

        [JsonProperty(@"number_elevator_bank", NullValueHandling = NullValueHandling.Ignore)]
        public int? number_elevator_bank { get; set; }

        [JsonProperty(@"number_of_passanger_elevator", NullValueHandling = NullValueHandling.Ignore)]
        public int? number_of_passanger_elevator { get; set; }

        [JsonProperty(@"passanger_elevator_capacity", NullValueHandling = NullValueHandling.Ignore)]
        public int? passanger_elevator_capacity { get; set; }

        [JsonProperty(@"passanger_elevator_capacity_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string passanger_elevator_capacity_uom_desc { get; set; }

        [JsonProperty(@"number_freight_elevator", NullValueHandling = NullValueHandling.Ignore)]
        public int? number_freight_elevator { get; set; }

        [JsonProperty(@"freight_elevator_capacity", NullValueHandling = NullValueHandling.Ignore)]
        public int? freight_elevator_capacity { get; set; }

        [JsonProperty(@"freight_elevator_capacity_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string freight_elevator_capacity_uom_desc { get; set; }

        [JsonProperty(@"cam_charge", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? cam_charge { get; set; }

        [JsonProperty(@"ref_cam_charge_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_cam_charge_uom_desc { get; set; }

        [JsonProperty(@"iron_mountain_file_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? iron_mountain_file_f { get; set; }

        [JsonProperty(@"block_and_tract_number", NullValueHandling = NullValueHandling.Ignore)]
        public string block_and_tract_number { get; set; }

        [JsonProperty(@"lot_number", NullValueHandling = NullValueHandling.Ignore)]
        public string lot_number { get; set; }

        [JsonProperty(@"minimum_floor_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? minimum_floor_area { get; set; }

        [JsonProperty(@"minimum_floor_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string minimum_floor_area_uom_desc { get; set; }

        [JsonProperty(@"maximum_floor_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? maximum_floor_area { get; set; }

        [JsonProperty(@"maximum_floor_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string maximum_floor_area_uom_desc { get; set; }

        [JsonProperty(@"typical_floor_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? typical_floor_area { get; set; }

        [JsonProperty(@"typical_floor_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string typical_floor_area_uom_desc { get; set; }

        [JsonProperty(@"ref_property_sprinkler_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_property_sprinkler_type_desc { get; set; }

        [JsonProperty(@"terrace_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? terrace_area { get; set; }

        [JsonProperty(@"terrace_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string terrace_area_uom_desc { get; set; }

        [JsonProperty(@"premier_property_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? premier_property_f { get; set; }

        [JsonProperty(@"ref_built_to_specification_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_built_to_specification_desc { get; set; }

        [JsonProperty(@"ref_data_acquired_from_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_data_acquired_from_desc { get; set; }

        [JsonProperty(@"floor_area_ratio", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? floor_area_ratio { get; set; }

        [JsonProperty(@"loss_factor", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? loss_factor { get; set; }

        [JsonProperty(@"lot_coverage", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? lot_coverage { get; set; }

        [JsonProperty(@"ref_possession_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_possession_type_desc { get; set; }

        [JsonProperty(@"total_number_of_floors", NullValueHandling = NullValueHandling.Ignore)]
        public int? total_number_of_floors { get; set; }

        [JsonProperty(@"ref_property_tenancy_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_property_tenancy_type_desc { get; set; }

        [JsonProperty(@"property_plant_and_machinery_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? property_plant_and_machinery_f { get; set; }

        [JsonProperty(@"building_common_name", NullValueHandling = NullValueHandling.Ignore)]
        public string building_common_name { get; set; }

        [JsonProperty(@"month_property_renovated", NullValueHandling = NullValueHandling.Ignore)]
        public int? month_property_renovated { get; set; }

        [JsonProperty(@"year_property_renovated", NullValueHandling = NullValueHandling.Ignore)]
        public int? year_property_renovated { get; set; }

        [JsonProperty(@"cbre_managed_property_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? cbre_managed_property_f { get; set; }

        [JsonProperty(@"occupancy_rate", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? occupancy_rate { get; set; }

        [JsonProperty(@"prime_property_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? prime_property_f { get; set; }

        [JsonProperty(@"property_class_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string property_class_type_desc { get; set; }

        [JsonProperty(@"property_office_park_name", NullValueHandling = NullValueHandling.Ignore)]
        public string property_office_park_name { get; set; }

        [JsonProperty(@"cam_charge_note", NullValueHandling = NullValueHandling.Ignore)]
        public string cam_charge_note { get; set; }

        [JsonProperty(@"record_source_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string record_source_desc { get; set; }

        [JsonProperty(@"dig_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string dig_notes { get; set; }

        [JsonProperty(@"parking_comment", NullValueHandling = NullValueHandling.Ignore)]
        public string parking_comment { get; set; }

        [JsonProperty(@"record_source_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string record_source_notes { get; set; }

        [JsonProperty(@"source_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string source_notes { get; set; }

        [JsonProperty(@"year_construction_started", NullValueHandling = NullValueHandling.Ignore)]
        public int? year_construction_started { get; set; }

        [JsonProperty(@"month_construction_started", NullValueHandling = NullValueHandling.Ignore)]
        public int? month_construction_started { get; set; }

        [JsonProperty(@"floor_loading_capacity", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? floor_loading_capacity { get; set; }

        [JsonProperty(@"floor_loading_capacity_uom", NullValueHandling = NullValueHandling.Ignore)]
        public string floor_loading_capacity_uom { get; set; }

        [JsonProperty(@"property_status_change_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? property_status_change_date { get; set; }

        ///<summary>
        /// deprecated once MIQ is not using this column
        ///</summary>
        [JsonProperty(@"floor_loading_capacity_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string floor_loading_capacity_uom_desc { get; set; }

        [JsonProperty(@"associated_project_completion_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? associated_project_completion_date { get; set; }

        [JsonProperty(@"number_of_floor_below_ground", NullValueHandling = NullValueHandling.Ignore)]
        public int? number_of_floor_below_ground { get; set; }

        [JsonProperty(@"number_of_floor_above_ground", NullValueHandling = NullValueHandling.Ignore)]
        public int? number_of_floor_above_ground { get; set; }

        [JsonProperty(@"power_allocation_per_floor", NullValueHandling = NullValueHandling.Ignore)]
        public int? power_allocation_per_floor { get; set; }

        [JsonProperty(@"power_allocation_per_floor_uom", NullValueHandling = NullValueHandling.Ignore)]
        public string power_allocation_per_floor_uom { get; set; }

        [JsonProperty(@"property_ownership_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string property_ownership_type_desc { get; set; }

        [JsonProperty(@"power_allocation_for_building", NullValueHandling = NullValueHandling.Ignore)]
        public int? power_allocation_for_building { get; set; }

        [JsonProperty(@"power_allocation_for_building_uom", NullValueHandling = NullValueHandling.Ignore)]
        public string power_allocation_for_building_uom { get; set; }

        [JsonProperty(@"usages", Required = Required.Always)]
        public List<PropertyUsage> usages { get; set; }

        [JsonProperty(@"postal_address", Required = Required.Always)]
        public PostalAddress postal_address { get; set; }

        [JsonProperty(@"elevator_bank", NullValueHandling = NullValueHandling.Ignore)]
        public List<ElevatorBankInfo> elevator_bank { get; set; }

        [JsonProperty(@"property_note", NullValueHandling = NullValueHandling.Ignore)]
        public List<PropertyNote> property_note { get; set; }

        [JsonProperty(@"digital", NullValueHandling = NullValueHandling.Ignore)]
        public List<Digital> digital { get; set; }

        [JsonProperty(@"property_media_information", NullValueHandling = NullValueHandling.Ignore)]
        public List<PropertyMediaInformation> property_media_information { get; set; }

        [JsonProperty(@"property_external_rating", NullValueHandling = NullValueHandling.Ignore)]
        public PropertyExternalRating property_external_rating { get; set; }

        [JsonProperty(@"property_external_ratings", NullValueHandling = NullValueHandling.Ignore)]
        public List<PropertyExternalRating> property_external_ratings { get; set; }

        [JsonProperty(@"property_apn_number", NullValueHandling = NullValueHandling.Ignore)]
        public List<PropertyApnNumber> property_apn_number { get; set; }

        [JsonProperty(@"property_amenity", NullValueHandling = NullValueHandling.Ignore)]
        public List<PropertyAmenity> property_amenity { get; set; }

        [JsonProperty(@"property_tax_assessment", NullValueHandling = NullValueHandling.Ignore)]
        public List<PropertyTaxAssessment> property_tax_assessment { get; set; }

        [JsonProperty(@"asset", NullValueHandling = NullValueHandling.Ignore)]
        public List<Asset> asset { get; set; }

        [JsonProperty(@"property_elevator", NullValueHandling = NullValueHandling.Ignore)]
        public List<PropertyElevator> property_elevator { get; set; }

        [JsonProperty(@"property_air_condition", NullValueHandling = NullValueHandling.Ignore)]
        public List<PropertyAirCondition> property_air_condition { get; set; }

        [JsonProperty(@"colloquial", NullValueHandling = NullValueHandling.Ignore)]
        public List<PropertyColloquialDetail> colloquial { get; set; }

        [JsonProperty(@"space_to_office_conversion_efficiency_rate", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? space_to_office_conversion_efficiency_rate { get; set; }

        [JsonProperty(@"space_utilization_efficiency_rate", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? space_utilization_efficiency_rate { get; set; }

        [JsonProperty(@"user_tag", NullValueHandling = NullValueHandling.Ignore)]
        public List<UserTag> user_tag { get; set; }

        [JsonProperty(@"property_landlord_provision", NullValueHandling = NullValueHandling.Ignore)]
        public List<PropertyLandlordProvision> property_landlord_provision { get; set; }

        [JsonProperty(@"property_measurement", NullValueHandling = NullValueHandling.Ignore)]
        public List<PropertyMeasurement> property_measurement { get; set; }

        [JsonProperty(@"property_frontage", NullValueHandling = NullValueHandling.Ignore)]
        public List<PropertyFrontage> property_frontage { get; set; }

        [JsonProperty(@"source_details", NullValueHandling = NullValueHandling.Ignore)]
        public string source_details { get; set; }

        [JsonProperty(@"miq_record_hide_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? miq_record_hide_f { get; set; }
    }
}
