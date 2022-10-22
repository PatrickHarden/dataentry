using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class PropertyUpdate : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{property_name,total_land_area,total_land_area_uom_desc,property_descr,property_zoning_code,property_notes,last_sale_price,last_sale_price_uom_desc,last_purchase_date,owner_occupied_f,total_gross_area,total_gross_area_uom_desc,net_rentable_area,net_rentable_area_uom_desc,ref_property_status_desc,year_property_built,month_property_built,statistical_f,cbre_owned_property_f,number_parking_space,parking_ratio,operating_expense,operating_expense_uom_desc,operating_expense_year,ref_property_construction_type_desc,number_elevator_bank,number_of_passanger_elevator,passanger_elevator_capacity,passanger_elevator_capacity_uom_desc,number_freight_elevator,freight_elevator_capacity,freight_elevator_capacity_uom_desc,cam_charge,ref_cam_charge_uom_desc,iron_mountain_file_f,block_and_tract_number,lot_number,minimum_floor_area,minimum_floor_area_uom_desc,maximum_floor_area,maximum_floor_area_uom_desc,typical_floor_area,typical_floor_area_uom_desc,ref_property_sprinkler_type_desc,terrace_area,terrace_area_uom_desc,premier_property_f,ref_built_to_specification_desc,ref_data_acquired_from_desc,floor_area_ratio,loss_factor,lot_coverage,ref_possession_type_desc,total_number_of_floors,number_of_floor_below_ground,ref_property_tenancy_type_desc,property_plant_and_machinery_f,building_common_name,month_property_renovated,year_property_renovated,cbre_managed_property_f,occupancy_rate,prime_property_f,property_class_type_desc,property_office_park_name,floor_loading_capacity,floor_loading_capacity_uom,property_status_change_date,floor_loading_capacity_uom_desc,associated_project_completion_date,cam_charge_note,record_source_desc,dig_notes,parking_comment,record_source_notes,source_notes,verified_ts,verified_by,latitude,longitude,year_construction_started,month_construction_started,number_of_floor_above_ground,power_allocation_per_floor,power_allocation_per_floor_uom,property_ownership_type_desc,power_allocation_for_building,power_allocation_for_building_uom,source_details,miq_record_hide_f}";

        [JsonProperty(@"property_name", NullValueHandling = NullValueHandling.Ignore)]
        public string property_name { get; set; }

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

        [JsonProperty(@"ref_property_status_desc", NullValueHandling = NullValueHandling.Ignore)]
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

        [JsonProperty(@"number_of_floor_below_ground", NullValueHandling = NullValueHandling.Ignore)]
        public int? number_of_floor_below_ground { get; set; }

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

        [JsonProperty(@"verified_ts", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? verified_ts { get; set; }

        [JsonProperty(@"verified_by", NullValueHandling = NullValueHandling.Ignore)]
        public string verified_by { get; set; }

        [JsonProperty(@"latitude", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? latitude { get; set; }

        [JsonProperty(@"longitude", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? longitude { get; set; }

        [JsonProperty(@"year_construction_started", NullValueHandling = NullValueHandling.Ignore)]
        public int? year_construction_started { get; set; }

        [JsonProperty(@"month_construction_started", NullValueHandling = NullValueHandling.Ignore)]
        public int? month_construction_started { get; set; }

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

        [JsonProperty(@"source_details", NullValueHandling = NullValueHandling.Ignore)]
        public string source_details { get; set; }

        [JsonProperty(@"miq_record_hide_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? miq_record_hide_f { get; set; }
    }
}
