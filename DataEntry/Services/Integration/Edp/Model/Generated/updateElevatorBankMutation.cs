using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class updateElevatorBankMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"updateElevatorBank"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"property_id", property_id);
                yield return new KeyValuePair<string, object>(@"property_elevator_bank_id", property_elevator_bank_id);
                yield return new KeyValuePair<string, object>(@"property_elevator_bank", property_elevator_bank);
            }
        }

        public RequestDetails request { get; set; }

        public int property_id { get; set; }

        public int property_elevator_bank_id { get; set; }

        public ElevatorBankInfoUpdate property_elevator_bank { get; set; }
    }
}
