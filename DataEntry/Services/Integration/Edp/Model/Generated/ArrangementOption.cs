using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class ArrangementOption : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{arrangement_option_type_desc,arrangement_option_start_date,arrangement_option_execution_date,arrangement_option_end_date,option_occurance,option_occurance_uom_desc}";

        [JsonProperty(@"arrangement_option_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string arrangement_option_type_desc { get; set; }

        [JsonProperty(@"arrangement_option_start_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? arrangement_option_start_date { get; set; }

        [JsonProperty(@"arrangement_option_execution_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? arrangement_option_execution_date { get; set; }

        [JsonProperty(@"arrangement_option_end_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? arrangement_option_end_date { get; set; }

        [JsonProperty(@"option_occurance", NullValueHandling = NullValueHandling.Ignore)]
        public int? option_occurance { get; set; }

        [JsonProperty(@"option_occurance_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string option_occurance_uom_desc { get; set; }
    }
}
