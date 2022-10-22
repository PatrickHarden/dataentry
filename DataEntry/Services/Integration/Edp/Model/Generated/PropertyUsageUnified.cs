using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class PropertyUsageUnified : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,ref_property_usage_type_desc,ref_property_usage_sub_type_desc,ref_property_class_type_desc,total_direct_available_surface_area,total_direct_available_surface_area_uom_desc,total_available_area_sub_lease,total_available_area_sub_lease_uom_desc,total_available_surface_area,total_available_surface_area_uom_desc,direct_vacant_area,direct_vacant_area_uom_desc,sublease_vacant_area,sublease_vacant_area_uom_desc,cbre_managed_direct_surface_area_available,cbre_managed_direct_surface_area_available_uom_desc,total_vacant_area,total_vacant_area_uom_desc,cbre_managed_sub_lease_surface_area_available,cbre_managed_sub_lease_surface_area_available_uom_desc,statistical_f,net_rentable_area,net_rentable_area_uom_desc,total_gross_area,total_gross_area_uom_desc,maximum_contiguous_surface_area,maximum_contiguous_surface_area_uom_desc,minimum_divisible_surface_area,minimum_divisible_surface_area_uom_desc}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public string id { get; set; }

        [JsonProperty(@"ref_property_usage_type_desc", Required = Required.Always)]
        public string ref_property_usage_type_desc { get; set; }

        [JsonProperty(@"ref_property_usage_sub_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_property_usage_sub_type_desc { get; set; }

        [JsonProperty(@"ref_property_class_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_property_class_type_desc { get; set; }

        [JsonProperty(@"total_direct_available_surface_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? total_direct_available_surface_area { get; set; }

        [JsonProperty(@"total_direct_available_surface_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string total_direct_available_surface_area_uom_desc { get; set; }

        [JsonProperty(@"total_available_area_sub_lease", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? total_available_area_sub_lease { get; set; }

        [JsonProperty(@"total_available_area_sub_lease_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string total_available_area_sub_lease_uom_desc { get; set; }

        [JsonProperty(@"total_available_surface_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? total_available_surface_area { get; set; }

        [JsonProperty(@"total_available_surface_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string total_available_surface_area_uom_desc { get; set; }

        [JsonProperty(@"direct_vacant_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? direct_vacant_area { get; set; }

        [JsonProperty(@"direct_vacant_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string direct_vacant_area_uom_desc { get; set; }

        [JsonProperty(@"sublease_vacant_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? sublease_vacant_area { get; set; }

        [JsonProperty(@"sublease_vacant_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string sublease_vacant_area_uom_desc { get; set; }

        [JsonProperty(@"cbre_managed_direct_surface_area_available", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? cbre_managed_direct_surface_area_available { get; set; }

        [JsonProperty(@"cbre_managed_direct_surface_area_available_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string cbre_managed_direct_surface_area_available_uom_desc { get; set; }

        [JsonProperty(@"total_vacant_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? total_vacant_area { get; set; }

        [JsonProperty(@"total_vacant_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string total_vacant_area_uom_desc { get; set; }

        [JsonProperty(@"cbre_managed_sub_lease_surface_area_available", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? cbre_managed_sub_lease_surface_area_available { get; set; }

        [JsonProperty(@"cbre_managed_sub_lease_surface_area_available_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string cbre_managed_sub_lease_surface_area_available_uom_desc { get; set; }

        [JsonProperty(@"statistical_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? statistical_f { get; set; }

        [JsonProperty(@"net_rentable_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? net_rentable_area { get; set; }

        [JsonProperty(@"net_rentable_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string net_rentable_area_uom_desc { get; set; }

        [JsonProperty(@"total_gross_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? total_gross_area { get; set; }

        [JsonProperty(@"total_gross_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string total_gross_area_uom_desc { get; set; }

        [JsonProperty(@"maximum_contiguous_surface_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? maximum_contiguous_surface_area { get; set; }

        [JsonProperty(@"maximum_contiguous_surface_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string maximum_contiguous_surface_area_uom_desc { get; set; }

        [JsonProperty(@"minimum_divisible_surface_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? minimum_divisible_surface_area { get; set; }

        [JsonProperty(@"minimum_divisible_surface_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string minimum_divisible_surface_area_uom_desc { get; set; }
    }
}
