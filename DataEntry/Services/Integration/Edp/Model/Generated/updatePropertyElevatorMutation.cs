using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class updatePropertyElevatorMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"updatePropertyElevator"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"property_id", property_id);
                yield return new KeyValuePair<string, object>(@"property_elevator_id", property_elevator_id);
                yield return new KeyValuePair<string, object>(@"property_elevator", property_elevator);
            }
        }

        public RequestDetails request { get; set; }

        public int property_id { get; set; }

        public int property_elevator_id { get; set; }

        public PropertyElevatorUpdate property_elevator { get; set; }
    }
}
