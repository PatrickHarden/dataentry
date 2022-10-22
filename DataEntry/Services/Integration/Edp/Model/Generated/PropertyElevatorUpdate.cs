using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class PropertyElevatorUpdate : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,number_of_elevator,elevator_capacity,elevator_capacity_uom,elevator_notes,elevator_presence_flag}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public int? id { get; set; }

        [JsonProperty(@"number_of_elevator", NullValueHandling = NullValueHandling.Ignore)]
        public int? number_of_elevator { get; set; }

        [JsonProperty(@"elevator_capacity", NullValueHandling = NullValueHandling.Ignore)]
        public int? elevator_capacity { get; set; }

        [JsonProperty(@"elevator_capacity_uom", NullValueHandling = NullValueHandling.Ignore)]
        public string elevator_capacity_uom { get; set; }

        [JsonProperty(@"elevator_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string elevator_notes { get; set; }

        [JsonProperty(@"elevator_presence_flag", NullValueHandling = NullValueHandling.Ignore)]
        public string elevator_presence_flag { get; set; }
    }
}
