using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class LeaseCompFloorNSuite : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{floor_number,suite_number,arrangement_space_area,ref_arrangement_space_area_uom_desc,full_floor_f,property_arrangement_status_type_desc}";

        [JsonProperty(@"floor_number", Required = Required.Always)]
        public string floor_number { get; set; }

        [JsonProperty(@"suite_number", Required = Required.Always)]
        public string suite_number { get; set; }

        [JsonProperty(@"arrangement_space_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? arrangement_space_area { get; set; }

        [JsonProperty(@"ref_arrangement_space_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_arrangement_space_area_uom_desc { get; set; }

        [JsonProperty(@"full_floor_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? full_floor_f { get; set; }

        [JsonProperty(@"property_arrangement_status_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string property_arrangement_status_type_desc { get; set; }
    }
}
