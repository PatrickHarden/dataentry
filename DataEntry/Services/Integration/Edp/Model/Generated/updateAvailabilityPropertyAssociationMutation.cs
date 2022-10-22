using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class updateAvailabilityPropertyAssociationMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"updateAvailabilityPropertyAssociation"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"availability_id", availability_id);
                yield return new KeyValuePair<string, object>(@"propAssociation", propAssociation);
            }
        }

        public RequestDetails request { get; set; }

        public int availability_id { get; set; }

        public AvailabilityPropertyAssociation propAssociation { get; set; }
    }
}
