using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class TenantPropertySpaceUpdate : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,floor_number,suite_number,leased_space_size,leased_space_size_uom_desc,lease_start_date,lease_expiration_date,tenancy_status,occupancy_date,headquarter_location_f,sublease_f,lease_id,ref_property_usage_type_desc,ref_property_usage_sub_type_desc,brand_id,estimated_rent_amount,estimated_rent_amount_uom,external_opportunity_number,external_opportunity_number_source_desc}";

        [JsonProperty(@"id", Required = Required.Always)]
        public int id { get; set; }

        ///<summary>
        /// deprecated  cant update natural key
        ///</summary>
        [JsonProperty(@"floor_number", NullValueHandling = NullValueHandling.Ignore)]
        public string floor_number { get; set; }

        ///<summary>
        /// deprecated  cant update natural key
        ///</summary>
        [JsonProperty(@"suite_number", NullValueHandling = NullValueHandling.Ignore)]
        public string suite_number { get; set; }

        [JsonProperty(@"leased_space_size", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? leased_space_size { get; set; }

        [JsonProperty(@"leased_space_size_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string leased_space_size_uom_desc { get; set; }

        [JsonProperty(@"lease_start_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? lease_start_date { get; set; }

        [JsonProperty(@"lease_expiration_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? lease_expiration_date { get; set; }

        [JsonProperty(@"tenancy_status", NullValueHandling = NullValueHandling.Ignore)]
        public string tenancy_status { get; set; }

        [JsonProperty(@"occupancy_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? occupancy_date { get; set; }

        [JsonProperty(@"headquarter_location_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? headquarter_location_f { get; set; }

        [JsonProperty(@"sublease_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? sublease_f { get; set; }

        [JsonProperty(@"lease_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? lease_id { get; set; }

        [JsonProperty(@"ref_property_usage_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_property_usage_type_desc { get; set; }

        [JsonProperty(@"ref_property_usage_sub_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_property_usage_sub_type_desc { get; set; }

        [JsonProperty(@"brand_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? brand_id { get; set; }

        [JsonProperty(@"estimated_rent_amount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? estimated_rent_amount { get; set; }

        [JsonProperty(@"estimated_rent_amount_uom", NullValueHandling = NullValueHandling.Ignore)]
        public string estimated_rent_amount_uom { get; set; }

        [JsonProperty(@"external_opportunity_number", NullValueHandling = NullValueHandling.Ignore)]
        public string external_opportunity_number { get; set; }

        [JsonProperty(@"external_opportunity_number_source_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string external_opportunity_number_source_desc { get; set; }
    }
}
