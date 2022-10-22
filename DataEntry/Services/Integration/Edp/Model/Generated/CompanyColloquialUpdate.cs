using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class CompanyColloquialUpdate : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{alternative_company_name}";

        [JsonProperty(@"alternative_company_name", NullValueHandling = NullValueHandling.Ignore)]
        public string alternative_company_name { get; set; }
    }
}
