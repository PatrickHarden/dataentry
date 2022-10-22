using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class Entity : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{entity_type,id,transaction_flag,source_lineage{source_system,source_unique_id}}";

        [JsonProperty(@"entity_type", NullValueHandling = NullValueHandling.Ignore)]
        public string entity_type { get; set; }

        [JsonProperty(@"id", Required = Required.Always)]
        public int id { get; set; }

        [JsonProperty(@"transaction_flag", NullValueHandling = NullValueHandling.Ignore)]
        public string transaction_flag { get; set; }

        [JsonProperty(@"source_lineage", NullValueHandling = NullValueHandling.Ignore)]
        public List<SourceLineage> source_lineage { get; set; }
    }
}
