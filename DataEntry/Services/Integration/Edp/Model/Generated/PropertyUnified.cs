using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class PropertyUnified : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,property_name,net_rentable_area,total_gross_area,total_number_of_floors,month_property_built,year_property_built,number_parking_space,parking_ratio,statistical_f,total_land_area,total_land_area_uom_desc,property_class_type_desc,property_status_desc,postal_address{id,street1,street2,street_name,pre_street_direction_name,street_type,post_street_direction_name,address_line_1,city,state_province,postal_code,postal_code_extension,county,country,latitude,longitude},property_usage{id,ref_property_usage_type_desc,ref_property_usage_sub_type_desc,ref_property_class_type_desc,total_direct_available_surface_area,total_direct_available_surface_area_uom_desc,total_available_area_sub_lease,total_available_area_sub_lease_uom_desc,total_available_surface_area,total_available_surface_area_uom_desc,direct_vacant_area,direct_vacant_area_uom_desc,sublease_vacant_area,sublease_vacant_area_uom_desc,cbre_managed_direct_surface_area_available,cbre_managed_direct_surface_area_available_uom_desc,total_vacant_area,total_vacant_area_uom_desc,cbre_managed_sub_lease_surface_area_available,cbre_managed_sub_lease_surface_area_available_uom_desc,statistical_f,net_rentable_area,net_rentable_area_uom_desc,total_gross_area,total_gross_area_uom_desc,maximum_contiguous_surface_area,maximum_contiguous_surface_area_uom_desc,minimum_divisible_surface_area,minimum_divisible_surface_area_uom_desc},property_amenity{id,property_amenity_type_desc,amenity_desc,amenity_notes,amenity_distance_from_property,amenity_distance_from_property_uom,amenity_purchase_price,amenity_purchase_price_uom},property_media_information{id,media_name,media_path,primary_image_f,ref_property_image_type_desc,still_video_f,ref_image_size_type_desc,ref_media_source_system_desc,publish_image_f,image_cbre_owned_f,media_caption,image_width,image_height,ref_image_orientation_desc,thumbnail_f,loopnet_image_f,allow_web_view_f,source_notes,ref_media_quality_type_desc,ref_media_view_type_desc,ref_property_usage_type_desc,media_type_desc,media_content_type_desc,display_order}}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public string id { get; set; }

        [JsonProperty(@"property_name", NullValueHandling = NullValueHandling.Ignore)]
        public string property_name { get; set; }

        [JsonProperty(@"net_rentable_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? net_rentable_area { get; set; }

        [JsonProperty(@"total_gross_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? total_gross_area { get; set; }

        [JsonProperty(@"total_number_of_floors", NullValueHandling = NullValueHandling.Ignore)]
        public int? total_number_of_floors { get; set; }

        [JsonProperty(@"month_property_built", NullValueHandling = NullValueHandling.Ignore)]
        public int? month_property_built { get; set; }

        [JsonProperty(@"year_property_built", NullValueHandling = NullValueHandling.Ignore)]
        public int? year_property_built { get; set; }

        [JsonProperty(@"number_parking_space", NullValueHandling = NullValueHandling.Ignore)]
        public int? number_parking_space { get; set; }

        [JsonProperty(@"parking_ratio", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? parking_ratio { get; set; }

        [JsonProperty(@"statistical_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? statistical_f { get; set; }

        [JsonProperty(@"total_land_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? total_land_area { get; set; }

        [JsonProperty(@"total_land_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string total_land_area_uom_desc { get; set; }

        [JsonProperty(@"property_class_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string property_class_type_desc { get; set; }

        [JsonProperty(@"property_status_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string property_status_desc { get; set; }

        [JsonProperty(@"postal_address", Required = Required.Always)]
        public PostalAddressUnified postal_address { get; set; }

        [JsonProperty(@"property_usage", NullValueHandling = NullValueHandling.Ignore)]
        public List<PropertyUsageUnified> property_usage { get; set; }

        [JsonProperty(@"property_amenity", NullValueHandling = NullValueHandling.Ignore)]
        public List<PropertyAmenityUnified> property_amenity { get; set; }

        [JsonProperty(@"property_media_information", NullValueHandling = NullValueHandling.Ignore)]
        public List<PropertyMediaInformationUnified> property_media_information { get; set; }
    }
}
