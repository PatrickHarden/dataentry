using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class PropertyAirCondition : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,property_air_condition_desc,air_condition_type_desc,property_air_condition_note,week_day_type_desc,start_time,end_time}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public int? id { get; set; }

        [JsonProperty(@"property_air_condition_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string property_air_condition_desc { get; set; }

        [JsonProperty(@"air_condition_type_desc", Required = Required.Always)]
        public string air_condition_type_desc { get; set; }

        [JsonProperty(@"property_air_condition_note", NullValueHandling = NullValueHandling.Ignore)]
        public string property_air_condition_note { get; set; }

        [JsonProperty(@"week_day_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string week_day_type_desc { get; set; }

        [JsonProperty(@"start_time", NullValueHandling = NullValueHandling.Ignore)]
        public string start_time { get; set; }

        [JsonProperty(@"end_time", NullValueHandling = NullValueHandling.Ignore)]
        public string end_time { get; set; }
    }
}
