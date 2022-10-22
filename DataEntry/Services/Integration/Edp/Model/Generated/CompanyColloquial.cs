using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class CompanyColloquial : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{alternative_company_name,country_code_desc,language_desc}";

        [JsonProperty(@"alternative_company_name", NullValueHandling = NullValueHandling.Ignore)]
        public string alternative_company_name { get; set; }

        [JsonProperty(@"country_code_desc", Required = Required.Always)]
        public string country_code_desc { get; set; }

        [JsonProperty(@"language_desc", Required = Required.Always)]
        public string language_desc { get; set; }
    }
}
