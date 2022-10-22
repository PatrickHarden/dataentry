using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class deleteLeaseRentEscalationMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"deleteLeaseRentEscalation"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"lease_id", lease_id);
                yield return new KeyValuePair<string, object>(@"rent_escalation_id", rent_escalation_id);
            }
        }

        public RequestDetails request { get; set; }

        public int lease_id { get; set; }

        public int rent_escalation_id { get; set; }
    }
}
