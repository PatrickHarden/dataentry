using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class deleteTIMUsagesMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"deleteTIMUsages"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"tim_id", tim_id);
                yield return new KeyValuePair<string, object>(@"usage_id", usage_id);
            }
        }

        public RequestDetails request { get; set; }

        public int tim_id { get; set; }

        public List<int> usage_id { get; set; }
    }
}
