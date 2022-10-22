using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class SourceLineage : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{source_system,source_unique_id}";

        [JsonProperty(@"source_system", Required = Required.Always)]
        public string source_system { get; set; }

        [JsonProperty(@"source_unique_id", Required = Required.Always)]
        public string source_unique_id { get; set; }
    }
}
