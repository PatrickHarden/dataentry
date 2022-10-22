using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class LeaseEscalation : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{escalation_value_amount,escalation_value_amount_uom,escalation_value_percent,escalation_order,escalation_period}";

        [JsonProperty(@"escalation_value_amount", NullValueHandling = NullValueHandling.Ignore)]
        public string escalation_value_amount { get; set; }

        [JsonProperty(@"escalation_value_amount_uom", NullValueHandling = NullValueHandling.Ignore)]
        public string escalation_value_amount_uom { get; set; }

        [JsonProperty(@"escalation_value_percent", NullValueHandling = NullValueHandling.Ignore)]
        public string escalation_value_percent { get; set; }

        [JsonProperty(@"escalation_order", Required = Required.Always)]
        public int escalation_order { get; set; }

        [JsonProperty(@"escalation_period", Required = Required.Always)]
        public string escalation_period { get; set; }
    }
}
