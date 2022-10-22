using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class ElevatorBankInfo : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,elevator_bank_name,total_number_of_elevators,start_floor,end_floor,elevator_bank_notes,colloquial{id,elevator_bank_name,elevator_bank_notes,country_code_desc,language_desc}}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public int? id { get; set; }

        [JsonProperty(@"elevator_bank_name", NullValueHandling = NullValueHandling.Ignore)]
        public string elevator_bank_name { get; set; }

        [JsonProperty(@"total_number_of_elevators", NullValueHandling = NullValueHandling.Ignore)]
        public int? total_number_of_elevators { get; set; }

        [JsonProperty(@"start_floor", NullValueHandling = NullValueHandling.Ignore)]
        public string start_floor { get; set; }

        [JsonProperty(@"end_floor", NullValueHandling = NullValueHandling.Ignore)]
        public string end_floor { get; set; }

        [JsonProperty(@"elevator_bank_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string elevator_bank_notes { get; set; }

        [JsonProperty(@"colloquial", NullValueHandling = NullValueHandling.Ignore)]
        public List<ElevatorInfoColloquial> colloquial { get; set; }
    }
}
