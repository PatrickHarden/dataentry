using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class ElevatorInfoColloquialUpdate : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,property_elevator_bank_name,property_elevator_bank_notes}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public int? id { get; set; }

        [JsonProperty(@"property_elevator_bank_name", NullValueHandling = NullValueHandling.Ignore)]
        public string property_elevator_bank_name { get; set; }

        [JsonProperty(@"property_elevator_bank_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string property_elevator_bank_notes { get; set; }
    }
}
