using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class AvailabilityUnified : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,date_available,date_on_market,statistical_listing_f,listing_descr,listing_expiration_date,listing_notes,listing_source,vacant_f,amperage,asking_rental_rate_monthly,asking_rental_rate_monthly_uom_desc,asking_rental_rate_yearly,asking_rental_rate_yearly_uom_desc,asking_rental_rate_yearly_area_uom_desc,available_office_space,available_office_space_uom_desc,bay_count,bay_depth,bay_width,ceiling_height,clear_height,clear_height_uom_desc,maximum_clear_height,maximum_clear_height_uom_desc,minimum_clear_height,minimum_clear_height_uom_desc,minimum_divisible_surface_area,minimum_divisible_surface_area_uom_desc,number_of_dock_doors,number_of_grade_level_door,number_of_rail_door,electric_type_id,number_of_month_free_rent,frontage_descr,cooking_area_f,dock_door_f,grade_level_iron_door_f,door_rail_f,yard_space_available_f,inclusion_amount,inclusion_amount_uom_desc,corner_space_f,food_permitted_f,side_of_street_f,vent_available_f,maximum_lease_term,minimum_lease_term,parking_cost,parking_cost_uom_desc,parking_cost_notes,percentage_rent,ref_rail_service_type_desc,available_space,available_space_uom_desc,sub_divide_allowed_f,sub_lease_expire_date,tenent_improvement_allowance,tenent_improvement_allowance_uom_desc,tenent_improvement_allowance_text,total_area_of_space,total_area_of_space_uom_desc,voltage,asking_price_for_sale,asking_price_for_sale_uom_desc,ref_availability_type_desc,total_contiguous_area_of_space,total_contiguous_area_of_space_uom_desc,ref_possession_type_desc,ref_space_condition_type_desc,grade_level_door_f,ref_space_availability_status_desc,ref_off_market_reason_desc,retail_address_notes,user_configured_contiguous_space_f,ref_retail_location_type_desc,contiguous_space_number,ref_lease_type_desc,ref_sale_type_desc,ref_rate_type_desc,minimum_asking_rate_monthly,minimum_asking_rate_monthly_uom,maximum_asking_rate_monthly,maximum_asking_rate_monthly_uom,minimum_asking_rate_yearly,minimum_asking_rate_yearly_uom,maximum_asking_rate_yearly,maximum_asking_rate_yearly_uom,floor_number,suite_number,property_id,property{id,property_name,net_rentable_area,total_gross_area,total_number_of_floors,month_property_built,year_property_built,number_parking_space,parking_ratio,statistical_f,total_land_area,total_land_area_uom_desc,property_class_type_desc,property_status_desc,postal_address{id,street1,street2,street_name,pre_street_direction_name,street_type,post_street_direction_name,address_line_1,city,state_province,postal_code,postal_code_extension,county,country,latitude,longitude},property_usage{id,ref_property_usage_type_desc,ref_property_usage_sub_type_desc,ref_property_class_type_desc,total_direct_available_surface_area,total_direct_available_surface_area_uom_desc,total_available_area_sub_lease,total_available_area_sub_lease_uom_desc,total_available_surface_area,total_available_surface_area_uom_desc,direct_vacant_area,direct_vacant_area_uom_desc,sublease_vacant_area,sublease_vacant_area_uom_desc,cbre_managed_direct_surface_area_available,cbre_managed_direct_surface_area_available_uom_desc,total_vacant_area,total_vacant_area_uom_desc,cbre_managed_sub_lease_surface_area_available,cbre_managed_sub_lease_surface_area_available_uom_desc,statistical_f,net_rentable_area,net_rentable_area_uom_desc,total_gross_area,total_gross_area_uom_desc,maximum_contiguous_surface_area,maximum_contiguous_surface_area_uom_desc,minimum_divisible_surface_area,minimum_divisible_surface_area_uom_desc},property_amenity{id,property_amenity_type_desc,amenity_desc,amenity_notes,amenity_distance_from_property,amenity_distance_from_property_uom,amenity_purchase_price,amenity_purchase_price_uom},property_media_information{id,media_name,media_path,primary_image_f,ref_property_image_type_desc,still_video_f,ref_image_size_type_desc,ref_media_source_system_desc,publish_image_f,image_cbre_owned_f,media_caption,image_width,image_height,ref_image_orientation_desc,thumbnail_f,loopnet_image_f,allow_web_view_f,source_notes,ref_media_quality_type_desc,ref_media_view_type_desc,ref_property_usage_type_desc,media_type_desc,media_content_type_desc,display_order}},ref_property_usage_type_desc,listing_rep{id,name,postal_address{id,street1,street2,street_name,pre_street_direction_name,street_type,post_street_direction_name,address_line_1,city,state_province,postal_code,postal_code_extension,county,country,latitude,longitude},contact{id,name,email,phone},website,phone},carpet_area,carpet_area_uom_desc,common_area,common_area_uom_desc,cam_charges,cam_charges_uom_desc,global_listing_id,security_deposit_value,security_deposit_value_uom_desc}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public string id { get; set; }

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

        [JsonProperty(@"vacant_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? vacant_f { get; set; }

        [JsonProperty(@"amperage", NullValueHandling = NullValueHandling.Ignore)]
        public string amperage { get; set; }

        [JsonProperty(@"asking_rental_rate_monthly", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? asking_rental_rate_monthly { get; set; }

        [JsonProperty(@"asking_rental_rate_monthly_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string asking_rental_rate_monthly_uom_desc { get; set; }

        [JsonProperty(@"asking_rental_rate_yearly", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? asking_rental_rate_yearly { get; set; }

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

        [JsonProperty(@"maximum_lease_term", NullValueHandling = NullValueHandling.Ignore)]
        public string maximum_lease_term { get; set; }

        [JsonProperty(@"minimum_lease_term", NullValueHandling = NullValueHandling.Ignore)]
        public string minimum_lease_term { get; set; }

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

        [JsonProperty(@"ref_availability_type_desc", Required = Required.Always)]
        public string ref_availability_type_desc { get; set; }

        ///<summary>
        /// Lease or Sale
        ///</summary>
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

        [JsonProperty(@"floor_number", NullValueHandling = NullValueHandling.Ignore)]
        public string floor_number { get; set; }

        [JsonProperty(@"suite_number", NullValueHandling = NullValueHandling.Ignore)]
        public string suite_number { get; set; }

        [JsonProperty(@"property_id", NullValueHandling = NullValueHandling.Ignore)]
        public string property_id { get; set; }

        [JsonProperty(@"property", NullValueHandling = NullValueHandling.Ignore)]
        public PropertyUnified property { get; set; }

        [JsonProperty(@"ref_property_usage_type_desc", Required = Required.Always)]
        public string ref_property_usage_type_desc { get; set; }

        ///<summary>
        /// company and contact details
        ///</summary>
        [JsonProperty(@"listing_rep", Required = Required.Always)]
        public List<CompanyUnified> listing_rep { get; set; }

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

        [JsonProperty(@"global_listing_id", NullValueHandling = NullValueHandling.Ignore)]
        public string global_listing_id { get; set; }

        [JsonProperty(@"security_deposit_value", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? security_deposit_value { get; set; }

        [JsonProperty(@"security_deposit_value_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string security_deposit_value_uom_desc { get; set; }
    }
}
