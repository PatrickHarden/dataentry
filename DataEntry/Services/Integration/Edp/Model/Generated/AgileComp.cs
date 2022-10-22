using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class AgileComp : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{agile_deal,account_driven,cost_per_seat,cost_per_seat_uom}";

        [JsonProperty(@"agile_deal", NullValueHandling = NullValueHandling.Ignore)]
        public string agile_deal { get; set; }

        [JsonProperty(@"account_driven", NullValueHandling = NullValueHandling.Ignore)]
        public string account_driven { get; set; }

        [JsonProperty(@"cost_per_seat", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? cost_per_seat { get; set; }

        [JsonProperty(@"cost_per_seat_uom", NullValueHandling = NullValueHandling.Ignore)]
        public string cost_per_seat_uom { get; set; }
    }
}
