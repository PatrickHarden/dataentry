using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class PropertyUsage : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,ref_property_usage_type_desc,ref_property_usage_sub_type_desc,cbre_managed_direct_surface_area_available,cbre_managed_direct_surface_area_available_uom_desc,cbre_managed_sub_lease_surface_area_available,cbre_managed_sub_lease_surface_area_available_uom_desc,date_property_available,direct_vacant_area,direct_vacant_area_uom_desc,estimated_maximum_monthly_direct_lease_rate,estimated_maximum_monthly_direct_lease_rate_uom_desc,estimated_maximum_yearly_direct_lease_rate,estimated_maximum_yearly_direct_lease_rate_uom_desc,estimated_minimum_monthly_direct_lease_rate,estimated_minimum_monthly_direct_lease_rate_uom_desc,estimated_minimum_yearly_direct_lease_rate,estimated_minimum_yearly_direct_lease_rate_uom_desc,full_floor_available_f,maximum_contiguous_surface_area,maximum_contiguous_surface_area_uom_desc,maximum_lease_term,maximum_lease_term_uom_desc,maximum_monthly_direct_lease_rate,maximum_monthly_direct_lease_rate_uom_desc,maximum_yearly_direct_lease_rate,maximum_yearly_direct_lease_rate_uom_desc,minimum_divisible_surface,minimum_divisible_surface_area_uom_desc,minimum_lease_term,minimum_lease_term_uom_desc,minimum_monthly_direct_lease_rate,minimum_monthly_direct_lease_rate_uom_desc,minimum_monthly_sub_lease_rate,minimum_monthly_sub_lease_rate_uom_desc,total_available_area_sub_lease,total_available_area_sub_lease_uom_desc,total_available_surface_area,total_available_surface_area_uom_desc,total_direct_available_surface_area,total_direct_available_surface_area_uom_desc,total_vacant_area,total_vacant_area_uom_desc,net_rentable_area,net_rentable_area_uom_desc,total_gross_area,total_gross_area_uom_desc,ref_property_class_type_desc,primary_usage_f,ref_sublease_rate_type_desc,column_spacing_uom_desc,operating_expense_amount,operating_expense_amount_uom_desc,prime_property_f,yard_space_available_f,ref_direct_rate_type_desc,maximum_monthly_sub_lease_rate,maximum_monthly_sub_lease_rate_uom_desc,maximum_yearly_sub_lease_rate,maximum_yearly_sub_lease_rate_uom_desc,tenant_improvement_allowance_amount,tenant_improvement_allowance_amount_uom_desc,office_area,office_area_uom_desc,bay_size_length,bay_size_length_uom_desc,bay_size_width,bay_size_width_uom_desc,maximum_clear_height,maximum_clear_height_uom_desc,minimum_clear_height,minimum_clear_height_uom_desc,number_available_crane,crane_height,crane_height_uom_desc,crane_tonnage,crane_tonnage_uom_desc,crane_served_f,cross_dock_position_exist_f,number_cross_dock_position,door_rail_exist_f,number_doos_rail,excess_land_exists_f,exterior_position_exist_f,exterior_position_number,available_warehouse_area,available_warehouse_area_uom_desc,available_freezer_area,available_freezer_area_uom_desc,warehouse_air_conditioner_area,warehouse_air_conditioner_area_uom_desc,clean_room_area,clean_room_area_uom_desc,refrigerator_area,refrigerator_area_uom_desc,computer_area,computer_area_uom_desc,interior_position_exist_f,number_interior_position,number_bays,ref_rail_service_type_desc,truckwell_position_exist_f,number_truckwell_position,road_corridor,number_dock_door,dockdoor_exist_f,grade_doors_dock_number,grade_dock_door_exist_f,number_tenants,number_anchor_tenants,anchor_note,available_frontage_area,available_frontage_area_uom_desc,pads_available_f,voltage,amperage,depth_length,depth_length_uom_desc,flood_map_number,flood_plain,frontage_length,frontage_length_uom_desc,gas_type_desc,has_broadband_f,has_electrical_f,has_in_ground_tanks_f,has_telephone_f,heat_type_desc,institutional_f,insurance_cost,insurance_cost_uom_desc,lighting_system_desc,maintenence_cost,maintenence_cost_uom_desc,major_street_frontage,marketing_statistical_area,minimum_available_area,minimum_available_area_uom_desc,minimum_yearly_direct_lease_rate_amount,minimum_yearly_direct_lease_rate_amount_uom_desc,minimum_yearly_sub_lease_rate,minimum_yearly_sub_lease_rate_uom_desc,number_parcel,other_expense_amount,other_expense_amount_uom_desc,potential_uses,potential_zoning_code,price_per_unit_value,price_per_unit_value_uom_desc,primary_access,sewer_type_desc,stop_expense_amount,stop_expense_amount_uom_desc,sub_lease_vacant_area,sub_lease_vacant_area_uom_desc,surrounding_uses,topography_desc,total_area_not_available_vacant,total_area_not_available_vacant_uom_desc,direct_vacant_not_available_area,direct_vacant_not_available_area_uom_desc,sublease_vacant_not_available_area,sublease_vacant_not_available_area_uom_desc,total_number_of_unit,total_number_of_unit_uom_desc,total_expense,total_expense_uom_desc,units_per_area_value,units_per_area_value_uom_desc,utilities_expense_amount,utilities_expense_amount_uom_desc,vacancy_index_desc,source_verified_f,water_type_desc,available_land_area,available_land_area_uom_desc,corner_information,column_spacing,parking_type_desc,has_drive_through_dock_f,shared_access_f,bay_size_height,bay_size_height_uom_desc,additional_availability_notes,availability_notes,roof_comment,rail_provider_note,crane_note,hvac_comment,statistical_f,average_monthly_direct_lease_rate,average_monthly_direct_lease_rate_uom,average_monthly_sub_lease_rate,average_monthly_sub_lease_rate_uom,average_yearly_direct_lease_rate,average_yearly_direct_lease_rate_uom,average_yearly_sub_lease_rate,average_yearly_sub_lease_rate_uom,tenant_improvement_as_is_f,parking_expense_amount,parking_expense_amount_uom_desc,tax_expense_amount,tax_expense_amount_uom_desc,tax_expense_year,additional_property_usage_note,geography{global_region_desc,region_desc,market_desc,submarket_desc,district_desc,neighborhood_desc},lig_geography{id,global_region_id,global_region_desc,division_id,division_desc,market_area_id,market_area_desc,local_market_area_id,local_market_area_desc,market_id,market_desc,submarket_id,submarket_desc,district_id,district_desc,neighborhood_id,neighborhood_desc},usage_amenity{id,amenity_name,property_amenity_type_desc,amenity_desc,amenity_notes,amenity_distance_from_property,amenity_distance_from_property_uom,amenity_purchase_price,amenity_purchase_price_uom,size,colloquial{id,amenity_name,size,country_code_desc,language_desc,amenity_desc,amenity_notes}},property_parking{id,parking_comments,parking_type_desc,parking_sub_type_desc,parking_cost_monthly,parking_cost_monthly_uom,parking_cost_hourly,parking_cost_hourly_uom,parking_cost_hourly_start,parking_cost_hourly_start_uom,parking_cost_hourly_end,parking_cost_hourly_end_uom,parking_cost_monthly_start,parking_cost_monthly_start_uom,parking_cost_monthly_end,parking_cost_monthly_end_uom},company_contact_role_addresses{company_id,contact_id,role_desc,location_id},number_parking_space,colloquial{id,additional_property_usage_note,country_code_desc,language_desc},operating_expense_amount_yearly_f}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public int? id { get; set; }

        [JsonProperty(@"ref_property_usage_type_desc", Required = Required.Always)]
        public string ref_property_usage_type_desc { get; set; }

        [JsonProperty(@"ref_property_usage_sub_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_property_usage_sub_type_desc { get; set; }

        [JsonProperty(@"cbre_managed_direct_surface_area_available", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? cbre_managed_direct_surface_area_available { get; set; }

        [JsonProperty(@"cbre_managed_direct_surface_area_available_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string cbre_managed_direct_surface_area_available_uom_desc { get; set; }

        [JsonProperty(@"cbre_managed_sub_lease_surface_area_available", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? cbre_managed_sub_lease_surface_area_available { get; set; }

        [JsonProperty(@"cbre_managed_sub_lease_surface_area_available_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string cbre_managed_sub_lease_surface_area_available_uom_desc { get; set; }

        [JsonProperty(@"date_property_available", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? date_property_available { get; set; }

        [JsonProperty(@"direct_vacant_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? direct_vacant_area { get; set; }

        [JsonProperty(@"direct_vacant_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string direct_vacant_area_uom_desc { get; set; }

        [JsonProperty(@"estimated_maximum_monthly_direct_lease_rate", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? estimated_maximum_monthly_direct_lease_rate { get; set; }

        [JsonProperty(@"estimated_maximum_monthly_direct_lease_rate_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string estimated_maximum_monthly_direct_lease_rate_uom_desc { get; set; }

        [JsonProperty(@"estimated_maximum_yearly_direct_lease_rate", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? estimated_maximum_yearly_direct_lease_rate { get; set; }

        [JsonProperty(@"estimated_maximum_yearly_direct_lease_rate_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string estimated_maximum_yearly_direct_lease_rate_uom_desc { get; set; }

        [JsonProperty(@"estimated_minimum_monthly_direct_lease_rate", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? estimated_minimum_monthly_direct_lease_rate { get; set; }

        [JsonProperty(@"estimated_minimum_monthly_direct_lease_rate_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string estimated_minimum_monthly_direct_lease_rate_uom_desc { get; set; }

        [JsonProperty(@"estimated_minimum_yearly_direct_lease_rate", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? estimated_minimum_yearly_direct_lease_rate { get; set; }

        [JsonProperty(@"estimated_minimum_yearly_direct_lease_rate_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string estimated_minimum_yearly_direct_lease_rate_uom_desc { get; set; }

        [JsonProperty(@"full_floor_available_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? full_floor_available_f { get; set; }

        [JsonProperty(@"maximum_contiguous_surface_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? maximum_contiguous_surface_area { get; set; }

        [JsonProperty(@"maximum_contiguous_surface_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string maximum_contiguous_surface_area_uom_desc { get; set; }

        [JsonProperty(@"maximum_lease_term", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? maximum_lease_term { get; set; }

        [JsonProperty(@"maximum_lease_term_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string maximum_lease_term_uom_desc { get; set; }

        [JsonProperty(@"maximum_monthly_direct_lease_rate", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? maximum_monthly_direct_lease_rate { get; set; }

        [JsonProperty(@"maximum_monthly_direct_lease_rate_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string maximum_monthly_direct_lease_rate_uom_desc { get; set; }

        [JsonProperty(@"maximum_yearly_direct_lease_rate", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? maximum_yearly_direct_lease_rate { get; set; }

        [JsonProperty(@"maximum_yearly_direct_lease_rate_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string maximum_yearly_direct_lease_rate_uom_desc { get; set; }

        [JsonProperty(@"minimum_divisible_surface", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? minimum_divisible_surface { get; set; }

        [JsonProperty(@"minimum_divisible_surface_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string minimum_divisible_surface_area_uom_desc { get; set; }

        [JsonProperty(@"minimum_lease_term", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? minimum_lease_term { get; set; }

        [JsonProperty(@"minimum_lease_term_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string minimum_lease_term_uom_desc { get; set; }

        [JsonProperty(@"minimum_monthly_direct_lease_rate", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? minimum_monthly_direct_lease_rate { get; set; }

        [JsonProperty(@"minimum_monthly_direct_lease_rate_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string minimum_monthly_direct_lease_rate_uom_desc { get; set; }

        [JsonProperty(@"minimum_monthly_sub_lease_rate", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? minimum_monthly_sub_lease_rate { get; set; }

        [JsonProperty(@"minimum_monthly_sub_lease_rate_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string minimum_monthly_sub_lease_rate_uom_desc { get; set; }

        [JsonProperty(@"total_available_area_sub_lease", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? total_available_area_sub_lease { get; set; }

        [JsonProperty(@"total_available_area_sub_lease_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string total_available_area_sub_lease_uom_desc { get; set; }

        [JsonProperty(@"total_available_surface_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? total_available_surface_area { get; set; }

        [JsonProperty(@"total_available_surface_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string total_available_surface_area_uom_desc { get; set; }

        [JsonProperty(@"total_direct_available_surface_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? total_direct_available_surface_area { get; set; }

        [JsonProperty(@"total_direct_available_surface_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string total_direct_available_surface_area_uom_desc { get; set; }

        [JsonProperty(@"total_vacant_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? total_vacant_area { get; set; }

        [JsonProperty(@"total_vacant_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string total_vacant_area_uom_desc { get; set; }

        [JsonProperty(@"net_rentable_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? net_rentable_area { get; set; }

        [JsonProperty(@"net_rentable_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string net_rentable_area_uom_desc { get; set; }

        [JsonProperty(@"total_gross_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? total_gross_area { get; set; }

        [JsonProperty(@"total_gross_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string total_gross_area_uom_desc { get; set; }

        [JsonProperty(@"ref_property_class_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_property_class_type_desc { get; set; }

        [JsonProperty(@"primary_usage_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? primary_usage_f { get; set; }

        [JsonProperty(@"ref_sublease_rate_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_sublease_rate_type_desc { get; set; }

        [JsonProperty(@"column_spacing_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string column_spacing_uom_desc { get; set; }

        [JsonProperty(@"operating_expense_amount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? operating_expense_amount { get; set; }

        [JsonProperty(@"operating_expense_amount_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string operating_expense_amount_uom_desc { get; set; }

        [JsonProperty(@"prime_property_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? prime_property_f { get; set; }

        [JsonProperty(@"yard_space_available_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? yard_space_available_f { get; set; }

        [JsonProperty(@"ref_direct_rate_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_direct_rate_type_desc { get; set; }

        [JsonProperty(@"maximum_monthly_sub_lease_rate", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? maximum_monthly_sub_lease_rate { get; set; }

        [JsonProperty(@"maximum_monthly_sub_lease_rate_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string maximum_monthly_sub_lease_rate_uom_desc { get; set; }

        [JsonProperty(@"maximum_yearly_sub_lease_rate", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? maximum_yearly_sub_lease_rate { get; set; }

        [JsonProperty(@"maximum_yearly_sub_lease_rate_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string maximum_yearly_sub_lease_rate_uom_desc { get; set; }

        [JsonProperty(@"tenant_improvement_allowance_amount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? tenant_improvement_allowance_amount { get; set; }

        [JsonProperty(@"tenant_improvement_allowance_amount_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string tenant_improvement_allowance_amount_uom_desc { get; set; }

        [JsonProperty(@"office_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? office_area { get; set; }

        [JsonProperty(@"office_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string office_area_uom_desc { get; set; }

        [JsonProperty(@"bay_size_length", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? bay_size_length { get; set; }

        [JsonProperty(@"bay_size_length_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string bay_size_length_uom_desc { get; set; }

        [JsonProperty(@"bay_size_width", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? bay_size_width { get; set; }

        [JsonProperty(@"bay_size_width_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string bay_size_width_uom_desc { get; set; }

        [JsonProperty(@"maximum_clear_height", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? maximum_clear_height { get; set; }

        [JsonProperty(@"maximum_clear_height_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string maximum_clear_height_uom_desc { get; set; }

        [JsonProperty(@"minimum_clear_height", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? minimum_clear_height { get; set; }

        [JsonProperty(@"minimum_clear_height_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string minimum_clear_height_uom_desc { get; set; }

        [JsonProperty(@"number_available_crane", NullValueHandling = NullValueHandling.Ignore)]
        public int? number_available_crane { get; set; }

        [JsonProperty(@"crane_height", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? crane_height { get; set; }

        [JsonProperty(@"crane_height_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string crane_height_uom_desc { get; set; }

        [JsonProperty(@"crane_tonnage", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? crane_tonnage { get; set; }

        [JsonProperty(@"crane_tonnage_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string crane_tonnage_uom_desc { get; set; }

        [JsonProperty(@"crane_served_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? crane_served_f { get; set; }

        [JsonProperty(@"cross_dock_position_exist_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? cross_dock_position_exist_f { get; set; }

        [JsonProperty(@"number_cross_dock_position", NullValueHandling = NullValueHandling.Ignore)]
        public int? number_cross_dock_position { get; set; }

        [JsonProperty(@"door_rail_exist_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? door_rail_exist_f { get; set; }

        [JsonProperty(@"number_doos_rail", NullValueHandling = NullValueHandling.Ignore)]
        public int? number_doos_rail { get; set; }

        [JsonProperty(@"excess_land_exists_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? excess_land_exists_f { get; set; }

        [JsonProperty(@"exterior_position_exist_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? exterior_position_exist_f { get; set; }

        [JsonProperty(@"exterior_position_number", NullValueHandling = NullValueHandling.Ignore)]
        public int? exterior_position_number { get; set; }

        [JsonProperty(@"available_warehouse_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? available_warehouse_area { get; set; }

        [JsonProperty(@"available_warehouse_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string available_warehouse_area_uom_desc { get; set; }

        [JsonProperty(@"available_freezer_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? available_freezer_area { get; set; }

        [JsonProperty(@"available_freezer_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string available_freezer_area_uom_desc { get; set; }

        [JsonProperty(@"warehouse_air_conditioner_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? warehouse_air_conditioner_area { get; set; }

        [JsonProperty(@"warehouse_air_conditioner_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string warehouse_air_conditioner_area_uom_desc { get; set; }

        [JsonProperty(@"clean_room_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? clean_room_area { get; set; }

        [JsonProperty(@"clean_room_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string clean_room_area_uom_desc { get; set; }

        [JsonProperty(@"refrigerator_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? refrigerator_area { get; set; }

        [JsonProperty(@"refrigerator_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string refrigerator_area_uom_desc { get; set; }

        [JsonProperty(@"computer_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? computer_area { get; set; }

        [JsonProperty(@"computer_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string computer_area_uom_desc { get; set; }

        [JsonProperty(@"interior_position_exist_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? interior_position_exist_f { get; set; }

        [JsonProperty(@"number_interior_position", NullValueHandling = NullValueHandling.Ignore)]
        public int? number_interior_position { get; set; }

        [JsonProperty(@"number_bays", NullValueHandling = NullValueHandling.Ignore)]
        public int? number_bays { get; set; }

        [JsonProperty(@"ref_rail_service_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_rail_service_type_desc { get; set; }

        [JsonProperty(@"truckwell_position_exist_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? truckwell_position_exist_f { get; set; }

        [JsonProperty(@"number_truckwell_position", NullValueHandling = NullValueHandling.Ignore)]
        public int? number_truckwell_position { get; set; }

        [JsonProperty(@"road_corridor", NullValueHandling = NullValueHandling.Ignore)]
        public string road_corridor { get; set; }

        [JsonProperty(@"number_dock_door", NullValueHandling = NullValueHandling.Ignore)]
        public int? number_dock_door { get; set; }

        [JsonProperty(@"dockdoor_exist_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? dockdoor_exist_f { get; set; }

        [JsonProperty(@"grade_doors_dock_number", NullValueHandling = NullValueHandling.Ignore)]
        public int? grade_doors_dock_number { get; set; }

        [JsonProperty(@"grade_dock_door_exist_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? grade_dock_door_exist_f { get; set; }

        [JsonProperty(@"number_tenants", NullValueHandling = NullValueHandling.Ignore)]
        public int? number_tenants { get; set; }

        [JsonProperty(@"number_anchor_tenants", NullValueHandling = NullValueHandling.Ignore)]
        public int? number_anchor_tenants { get; set; }

        [JsonProperty(@"anchor_note", NullValueHandling = NullValueHandling.Ignore)]
        public string anchor_note { get; set; }

        [JsonProperty(@"available_frontage_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? available_frontage_area { get; set; }

        [JsonProperty(@"available_frontage_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string available_frontage_area_uom_desc { get; set; }

        [JsonProperty(@"pads_available_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? pads_available_f { get; set; }

        [JsonProperty(@"voltage", NullValueHandling = NullValueHandling.Ignore)]
        public string voltage { get; set; }

        [JsonProperty(@"amperage", NullValueHandling = NullValueHandling.Ignore)]
        public string amperage { get; set; }

        [JsonProperty(@"depth_length", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? depth_length { get; set; }

        [JsonProperty(@"depth_length_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string depth_length_uom_desc { get; set; }

        [JsonProperty(@"flood_map_number", NullValueHandling = NullValueHandling.Ignore)]
        public string flood_map_number { get; set; }

        [JsonProperty(@"flood_plain", NullValueHandling = NullValueHandling.Ignore)]
        public string flood_plain { get; set; }

        [JsonProperty(@"frontage_length", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? frontage_length { get; set; }

        [JsonProperty(@"frontage_length_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string frontage_length_uom_desc { get; set; }

        [JsonProperty(@"gas_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string gas_type_desc { get; set; }

        [JsonProperty(@"has_broadband_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? has_broadband_f { get; set; }

        [JsonProperty(@"has_electrical_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? has_electrical_f { get; set; }

        [JsonProperty(@"has_in_ground_tanks_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? has_in_ground_tanks_f { get; set; }

        [JsonProperty(@"has_telephone_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? has_telephone_f { get; set; }

        [JsonProperty(@"heat_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string heat_type_desc { get; set; }

        [JsonProperty(@"institutional_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? institutional_f { get; set; }

        [JsonProperty(@"insurance_cost", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? insurance_cost { get; set; }

        [JsonProperty(@"insurance_cost_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string insurance_cost_uom_desc { get; set; }

        [JsonProperty(@"lighting_system_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string lighting_system_desc { get; set; }

        [JsonProperty(@"maintenence_cost", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? maintenence_cost { get; set; }

        [JsonProperty(@"maintenence_cost_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string maintenence_cost_uom_desc { get; set; }

        [JsonProperty(@"major_street_frontage", NullValueHandling = NullValueHandling.Ignore)]
        public string major_street_frontage { get; set; }

        [JsonProperty(@"marketing_statistical_area", NullValueHandling = NullValueHandling.Ignore)]
        public string marketing_statistical_area { get; set; }

        [JsonProperty(@"minimum_available_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? minimum_available_area { get; set; }

        [JsonProperty(@"minimum_available_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string minimum_available_area_uom_desc { get; set; }

        [JsonProperty(@"minimum_yearly_direct_lease_rate_amount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? minimum_yearly_direct_lease_rate_amount { get; set; }

        [JsonProperty(@"minimum_yearly_direct_lease_rate_amount_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string minimum_yearly_direct_lease_rate_amount_uom_desc { get; set; }

        [JsonProperty(@"minimum_yearly_sub_lease_rate", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? minimum_yearly_sub_lease_rate { get; set; }

        [JsonProperty(@"minimum_yearly_sub_lease_rate_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string minimum_yearly_sub_lease_rate_uom_desc { get; set; }

        [JsonProperty(@"number_parcel", NullValueHandling = NullValueHandling.Ignore)]
        public int? number_parcel { get; set; }

        [JsonProperty(@"other_expense_amount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? other_expense_amount { get; set; }

        [JsonProperty(@"other_expense_amount_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string other_expense_amount_uom_desc { get; set; }

        [JsonProperty(@"potential_uses", NullValueHandling = NullValueHandling.Ignore)]
        public string potential_uses { get; set; }

        [JsonProperty(@"potential_zoning_code", NullValueHandling = NullValueHandling.Ignore)]
        public string potential_zoning_code { get; set; }

        [JsonProperty(@"price_per_unit_value", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? price_per_unit_value { get; set; }

        [JsonProperty(@"price_per_unit_value_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string price_per_unit_value_uom_desc { get; set; }

        [JsonProperty(@"primary_access", NullValueHandling = NullValueHandling.Ignore)]
        public string primary_access { get; set; }

        [JsonProperty(@"sewer_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string sewer_type_desc { get; set; }

        [JsonProperty(@"stop_expense_amount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? stop_expense_amount { get; set; }

        [JsonProperty(@"stop_expense_amount_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string stop_expense_amount_uom_desc { get; set; }

        [JsonProperty(@"sub_lease_vacant_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? sub_lease_vacant_area { get; set; }

        [JsonProperty(@"sub_lease_vacant_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string sub_lease_vacant_area_uom_desc { get; set; }

        [JsonProperty(@"surrounding_uses", NullValueHandling = NullValueHandling.Ignore)]
        public string surrounding_uses { get; set; }

        [JsonProperty(@"topography_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string topography_desc { get; set; }

        [JsonProperty(@"total_area_not_available_vacant", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? total_area_not_available_vacant { get; set; }

        [JsonProperty(@"total_area_not_available_vacant_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string total_area_not_available_vacant_uom_desc { get; set; }

        [JsonProperty(@"direct_vacant_not_available_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? direct_vacant_not_available_area { get; set; }

        [JsonProperty(@"direct_vacant_not_available_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string direct_vacant_not_available_area_uom_desc { get; set; }

        [JsonProperty(@"sublease_vacant_not_available_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? sublease_vacant_not_available_area { get; set; }

        [JsonProperty(@"sublease_vacant_not_available_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string sublease_vacant_not_available_area_uom_desc { get; set; }

        [JsonProperty(@"total_number_of_unit", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? total_number_of_unit { get; set; }

        [JsonProperty(@"total_number_of_unit_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string total_number_of_unit_uom_desc { get; set; }

        [JsonProperty(@"total_expense", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? total_expense { get; set; }

        [JsonProperty(@"total_expense_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string total_expense_uom_desc { get; set; }

        [JsonProperty(@"units_per_area_value", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? units_per_area_value { get; set; }

        [JsonProperty(@"units_per_area_value_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string units_per_area_value_uom_desc { get; set; }

        [JsonProperty(@"utilities_expense_amount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? utilities_expense_amount { get; set; }

        [JsonProperty(@"utilities_expense_amount_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string utilities_expense_amount_uom_desc { get; set; }

        [JsonProperty(@"vacancy_index_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string vacancy_index_desc { get; set; }

        [JsonProperty(@"source_verified_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? source_verified_f { get; set; }

        [JsonProperty(@"water_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string water_type_desc { get; set; }

        [JsonProperty(@"available_land_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? available_land_area { get; set; }

        [JsonProperty(@"available_land_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string available_land_area_uom_desc { get; set; }

        [JsonProperty(@"corner_information", NullValueHandling = NullValueHandling.Ignore)]
        public string corner_information { get; set; }

        [JsonProperty(@"column_spacing", NullValueHandling = NullValueHandling.Ignore)]
        public string column_spacing { get; set; }

        [JsonProperty(@"parking_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string parking_type_desc { get; set; }

        [JsonProperty(@"has_drive_through_dock_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? has_drive_through_dock_f { get; set; }

        [JsonProperty(@"shared_access_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? shared_access_f { get; set; }

        [JsonProperty(@"bay_size_height", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? bay_size_height { get; set; }

        [JsonProperty(@"bay_size_height_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string bay_size_height_uom_desc { get; set; }

        [JsonProperty(@"additional_availability_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string additional_availability_notes { get; set; }

        [JsonProperty(@"availability_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string availability_notes { get; set; }

        [JsonProperty(@"roof_comment", NullValueHandling = NullValueHandling.Ignore)]
        public string roof_comment { get; set; }

        [JsonProperty(@"rail_provider_note", NullValueHandling = NullValueHandling.Ignore)]
        public string rail_provider_note { get; set; }

        [JsonProperty(@"crane_note", NullValueHandling = NullValueHandling.Ignore)]
        public string crane_note { get; set; }

        [JsonProperty(@"hvac_comment", NullValueHandling = NullValueHandling.Ignore)]
        public string hvac_comment { get; set; }

        [JsonProperty(@"statistical_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? statistical_f { get; set; }

        [JsonProperty(@"average_monthly_direct_lease_rate", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? average_monthly_direct_lease_rate { get; set; }

        [JsonProperty(@"average_monthly_direct_lease_rate_uom", NullValueHandling = NullValueHandling.Ignore)]
        public string average_monthly_direct_lease_rate_uom { get; set; }

        [JsonProperty(@"average_monthly_sub_lease_rate", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? average_monthly_sub_lease_rate { get; set; }

        [JsonProperty(@"average_monthly_sub_lease_rate_uom", NullValueHandling = NullValueHandling.Ignore)]
        public string average_monthly_sub_lease_rate_uom { get; set; }

        [JsonProperty(@"average_yearly_direct_lease_rate", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? average_yearly_direct_lease_rate { get; set; }

        [JsonProperty(@"average_yearly_direct_lease_rate_uom", NullValueHandling = NullValueHandling.Ignore)]
        public string average_yearly_direct_lease_rate_uom { get; set; }

        [JsonProperty(@"average_yearly_sub_lease_rate", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? average_yearly_sub_lease_rate { get; set; }

        [JsonProperty(@"average_yearly_sub_lease_rate_uom", NullValueHandling = NullValueHandling.Ignore)]
        public string average_yearly_sub_lease_rate_uom { get; set; }

        [JsonProperty(@"tenant_improvement_as_is_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? tenant_improvement_as_is_f { get; set; }

        [JsonProperty(@"parking_expense_amount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? parking_expense_amount { get; set; }

        [JsonProperty(@"parking_expense_amount_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string parking_expense_amount_uom_desc { get; set; }

        [JsonProperty(@"tax_expense_amount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? tax_expense_amount { get; set; }

        [JsonProperty(@"tax_expense_amount_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string tax_expense_amount_uom_desc { get; set; }

        [JsonProperty(@"tax_expense_year", NullValueHandling = NullValueHandling.Ignore)]
        public int? tax_expense_year { get; set; }

        [JsonProperty(@"additional_property_usage_note", NullValueHandling = NullValueHandling.Ignore)]
        public string additional_property_usage_note { get; set; }

        [JsonProperty(@"geography", NullValueHandling = NullValueHandling.Ignore)]
        public Geography geography { get; set; }

        [JsonProperty(@"lig_geography", NullValueHandling = NullValueHandling.Ignore)]
        public LIGGeography lig_geography { get; set; }

        [JsonProperty(@"usage_amenity", NullValueHandling = NullValueHandling.Ignore)]
        public List<PropertyAmenity> usage_amenity { get; set; }

        [JsonProperty(@"property_parking", NullValueHandling = NullValueHandling.Ignore)]
        public List<PropertyParking> property_parking { get; set; }

        [JsonProperty(@"company_contact_role_addresses", NullValueHandling = NullValueHandling.Ignore)]
        public List<CompanyContactRoleAddress> company_contact_role_addresses { get; set; }

        [JsonProperty(@"number_parking_space", NullValueHandling = NullValueHandling.Ignore)]
        public int? number_parking_space { get; set; }

        [JsonProperty(@"colloquial", NullValueHandling = NullValueHandling.Ignore)]
        public List<PropertyUsageColloquial> colloquial { get; set; }

        [JsonProperty(@"operating_expense_amount_yearly_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? operating_expense_amount_yearly_f { get; set; }
    }
}
