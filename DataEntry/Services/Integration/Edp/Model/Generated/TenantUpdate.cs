using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class TenantUpdate : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{ref_naics_classification_desc,tenant_cbre_industry_code_desc,confidentiality_f,total_employee_at_current_location,total_area,total_area_uom_desc,verified_ts,verified_by,verified_f,data_source_notes,record_source_desc,data_acquired_from_desc,record_source_notes,tenancy_notes,miq_record_hide_f}";

        [JsonProperty(@"ref_naics_classification_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_naics_classification_desc { get; set; }

        [JsonProperty(@"tenant_cbre_industry_code_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string tenant_cbre_industry_code_desc { get; set; }

        [JsonProperty(@"confidentiality_f", NullValueHandling = NullValueHandling.Ignore)]
        public string confidentiality_f { get; set; }

        [JsonProperty(@"total_employee_at_current_location", NullValueHandling = NullValueHandling.Ignore)]
        public int? total_employee_at_current_location { get; set; }

        [JsonProperty(@"total_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? total_area { get; set; }

        [JsonProperty(@"total_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string total_area_uom_desc { get; set; }

        [JsonProperty(@"verified_ts", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? verified_ts { get; set; }

        [JsonProperty(@"verified_by", NullValueHandling = NullValueHandling.Ignore)]
        public string verified_by { get; set; }

        [JsonProperty(@"verified_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? verified_f { get; set; }

        [JsonProperty(@"data_source_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string data_source_notes { get; set; }

        [JsonProperty(@"record_source_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string record_source_desc { get; set; }

        [JsonProperty(@"data_acquired_from_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string data_acquired_from_desc { get; set; }

        [JsonProperty(@"record_source_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string record_source_notes { get; set; }

        [JsonProperty(@"tenancy_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string tenancy_notes { get; set; }

        [JsonProperty(@"miq_record_hide_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? miq_record_hide_f { get; set; }
    }
}
