using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class IndustryClassification : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{request{request_id,action,source_system_name,source_id,source_submitter_name,source_submitter_email,user_role,comment,global_region,country},company_industry_classification{id,primary_naics_code_f,ref_naics_classification_desc,ref_cbre_industry_code_desc}}";

        [JsonProperty(@"request", Required = Required.Always)]
        public RequestDetails request { get; set; }

        [JsonProperty(@"company_industry_classification", Required = Required.Always)]
        public CompanyIndustryClassification company_industry_classification { get; set; }
    }
}
