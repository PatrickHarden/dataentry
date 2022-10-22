using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class CompanyContactRoleAddress : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{company_id,contact_id,role_desc,location_id}";

        [JsonProperty(@"company_id", Required = Required.Always)]
        public int company_id { get; set; }

        [JsonProperty(@"contact_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? contact_id { get; set; }

        [JsonProperty(@"role_desc", Required = Required.Always)]
        public string role_desc { get; set; }

        [JsonProperty(@"location_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? location_id { get; set; }
    }
}
