using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class ElevatorBankInfoUpdate : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,property_elevator_bank_name,number_of_elevator_in_bank,elevator_bank_low_floor_number,elevator_bank_high_floor_number,property_elevator_bank_notes,colloquial{id,property_elevator_bank_name,property_elevator_bank_notes}}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public int? id { get; set; }

        [JsonProperty(@"property_elevator_bank_name", NullValueHandling = NullValueHandling.Ignore)]
        public string property_elevator_bank_name { get; set; }

        [JsonProperty(@"number_of_elevator_in_bank", NullValueHandling = NullValueHandling.Ignore)]
        public int? number_of_elevator_in_bank { get; set; }

        [JsonProperty(@"elevator_bank_low_floor_number", NullValueHandling = NullValueHandling.Ignore)]
        public string elevator_bank_low_floor_number { get; set; }

        [JsonProperty(@"elevator_bank_high_floor_number", NullValueHandling = NullValueHandling.Ignore)]
        public string elevator_bank_high_floor_number { get; set; }

        [JsonProperty(@"property_elevator_bank_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string property_elevator_bank_notes { get; set; }

        [JsonProperty(@"colloquial", NullValueHandling = NullValueHandling.Ignore)]
        public List<ElevatorInfoColloquialUpdate> colloquial { get; set; }
    }
}
