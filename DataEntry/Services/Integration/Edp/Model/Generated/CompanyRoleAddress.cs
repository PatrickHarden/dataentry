using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class CompanyRoleAddress : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{company_id,role_desc,location_id,effective_date}";

        [JsonProperty(@"company_id", Required = Required.Always)]
        public int company_id { get; set; }

        [JsonProperty(@"role_desc", Required = Required.Always)]
        public string role_desc { get; set; }

        [JsonProperty(@"location_id", Required = Required.Always)]
        public int location_id { get; set; }

        [JsonProperty(@"effective_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? effective_date { get; set; }
    }
}
