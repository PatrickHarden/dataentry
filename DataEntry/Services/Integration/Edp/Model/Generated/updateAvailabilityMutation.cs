using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class updateAvailabilityMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"updateAvailability"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"availability_id", availability_id);
                yield return new KeyValuePair<string, object>(@"availability", availability);
            }
        }

        public RequestDetails request { get; set; }

        public int availability_id { get; set; }

        public AvailabilityUpdate availability { get; set; }
    }
}
