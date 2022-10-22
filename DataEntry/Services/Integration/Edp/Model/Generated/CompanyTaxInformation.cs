using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class CompanyTaxInformation : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,party_tax_id,tax_code_type_desc,country_desc}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public int? id { get; set; }

        [JsonProperty(@"party_tax_id", Required = Required.Always)]
        public string party_tax_id { get; set; }

        [JsonProperty(@"tax_code_type_desc", Required = Required.Always)]
        public string tax_code_type_desc { get; set; }

        [JsonProperty(@"country_desc", Required = Required.Always)]
        public string country_desc { get; set; }
    }
}
