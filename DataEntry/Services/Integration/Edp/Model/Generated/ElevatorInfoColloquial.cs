using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class ElevatorInfoColloquial : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,elevator_bank_name,elevator_bank_notes,country_code_desc,language_desc}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public int? id { get; set; }

        [JsonProperty(@"elevator_bank_name", NullValueHandling = NullValueHandling.Ignore)]
        public string elevator_bank_name { get; set; }

        [JsonProperty(@"elevator_bank_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string elevator_bank_notes { get; set; }

        [JsonProperty(@"country_code_desc", Required = Required.Always)]
        public string country_code_desc { get; set; }

        [JsonProperty(@"language_desc", Required = Required.Always)]
        public string language_desc { get; set; }
    }
}
