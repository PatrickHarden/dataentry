using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class CompanyIndustryClassification : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,primary_naics_code_f,ref_naics_classification_desc,ref_cbre_industry_code_desc}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public int? id { get; set; }

        [JsonProperty(@"primary_naics_code_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? primary_naics_code_f { get; set; }

        [JsonProperty(@"ref_naics_classification_desc", Required = Required.Always)]
        public string ref_naics_classification_desc { get; set; }

        [JsonProperty(@"ref_cbre_industry_code_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_cbre_industry_code_desc { get; set; }
    }
}
