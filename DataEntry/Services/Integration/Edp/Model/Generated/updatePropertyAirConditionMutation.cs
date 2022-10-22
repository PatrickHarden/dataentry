using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class updatePropertyAirConditionMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"updatePropertyAirCondition"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"property_id", property_id);
                yield return new KeyValuePair<string, object>(@"property_air_condition_id", property_air_condition_id);
                yield return new KeyValuePair<string, object>(@"property_air_condition", property_air_condition);
            }
        }

        public RequestDetails request { get; set; }

        public int property_id { get; set; }

        public int property_air_condition_id { get; set; }

        public PropertyAirConditionUpdate property_air_condition { get; set; }
    }
}
