using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class PropertyTaxAssessment : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,tax_assessment_year,tax_amount,tax_amount_uom_desc,assessed_value_for_improvement,assessed_value_for_improvement_uom_desc,assessed_value_for_land,assessed_value_for_land_uom_desc,total_assessed_value_for_property,total_assessed_value_for_property_uom_desc}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public int? id { get; set; }

        [JsonProperty(@"tax_assessment_year", Required = Required.Always)]
        public int tax_assessment_year { get; set; }

        [JsonProperty(@"tax_amount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? tax_amount { get; set; }

        [JsonProperty(@"tax_amount_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string tax_amount_uom_desc { get; set; }

        [JsonProperty(@"assessed_value_for_improvement", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? assessed_value_for_improvement { get; set; }

        [JsonProperty(@"assessed_value_for_improvement_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string assessed_value_for_improvement_uom_desc { get; set; }

        [JsonProperty(@"assessed_value_for_land", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? assessed_value_for_land { get; set; }

        [JsonProperty(@"assessed_value_for_land_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string assessed_value_for_land_uom_desc { get; set; }

        [JsonProperty(@"total_assessed_value_for_property", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? total_assessed_value_for_property { get; set; }

        [JsonProperty(@"total_assessed_value_for_property_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string total_assessed_value_for_property_uom_desc { get; set; }
    }
}
