using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class PropertyParkingUpdate : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,parking_comments,parking_cost_monthly,parking_cost_monthly_uom,parking_cost_hourly,parking_cost_hourly_uom,parking_cost_hourly_start,parking_cost_hourly_start_uom,parking_cost_hourly_end,parking_cost_hourly_end_uom,parking_cost_monthly_start,parking_cost_monthly_start_uom,parking_cost_monthly_end,parking_cost_monthly_end_uom}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public int? id { get; set; }

        [JsonProperty(@"parking_comments", NullValueHandling = NullValueHandling.Ignore)]
        public string parking_comments { get; set; }

        [JsonProperty(@"parking_cost_monthly", NullValueHandling = NullValueHandling.Ignore)]
        public int? parking_cost_monthly { get; set; }

        [JsonProperty(@"parking_cost_monthly_uom", NullValueHandling = NullValueHandling.Ignore)]
        public string parking_cost_monthly_uom { get; set; }

        [JsonProperty(@"parking_cost_hourly", NullValueHandling = NullValueHandling.Ignore)]
        public int? parking_cost_hourly { get; set; }

        [JsonProperty(@"parking_cost_hourly_uom", NullValueHandling = NullValueHandling.Ignore)]
        public string parking_cost_hourly_uom { get; set; }

        [JsonProperty(@"parking_cost_hourly_start", NullValueHandling = NullValueHandling.Ignore)]
        public int? parking_cost_hourly_start { get; set; }

        [JsonProperty(@"parking_cost_hourly_start_uom", NullValueHandling = NullValueHandling.Ignore)]
        public string parking_cost_hourly_start_uom { get; set; }

        [JsonProperty(@"parking_cost_hourly_end", NullValueHandling = NullValueHandling.Ignore)]
        public int? parking_cost_hourly_end { get; set; }

        [JsonProperty(@"parking_cost_hourly_end_uom", NullValueHandling = NullValueHandling.Ignore)]
        public string parking_cost_hourly_end_uom { get; set; }

        [JsonProperty(@"parking_cost_monthly_start", NullValueHandling = NullValueHandling.Ignore)]
        public int? parking_cost_monthly_start { get; set; }

        [JsonProperty(@"parking_cost_monthly_start_uom", NullValueHandling = NullValueHandling.Ignore)]
        public string parking_cost_monthly_start_uom { get; set; }

        [JsonProperty(@"parking_cost_monthly_end", NullValueHandling = NullValueHandling.Ignore)]
        public int? parking_cost_monthly_end { get; set; }

        [JsonProperty(@"parking_cost_monthly_end_uom", NullValueHandling = NullValueHandling.Ignore)]
        public string parking_cost_monthly_end_uom { get; set; }
    }
}
