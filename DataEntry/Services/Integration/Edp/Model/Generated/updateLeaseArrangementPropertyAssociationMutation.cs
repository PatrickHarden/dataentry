using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class updateLeaseArrangementPropertyAssociationMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"updateLeaseArrangementPropertyAssociation"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"lease_id", lease_id);
                yield return new KeyValuePair<string, object>(@"propAssociation", propAssociation);
            }
        }

        public RequestDetails request { get; set; }

        public int lease_id { get; set; }

        public LeasePropertyAssociation propAssociation { get; set; }
    }
}
