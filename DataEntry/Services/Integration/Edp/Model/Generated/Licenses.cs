using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class Licenses : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{license_number,license_issuing_entity,license_expiry_date}";

        [JsonProperty(@"license_number", Required = Required.Always)]
        public string license_number { get; set; }

        [JsonProperty(@"license_issuing_entity", Required = Required.Always)]
        public string license_issuing_entity { get; set; }

        [JsonProperty(@"license_expiry_date", Required = Required.Always)]
        public DateTime license_expiry_date { get; set; }
    }
}
