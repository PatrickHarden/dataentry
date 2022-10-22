using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class updateLeaseArrangementOptionMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"updateLeaseArrangementOption"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"lease_id", lease_id);
                yield return new KeyValuePair<string, object>(@"arrangement_option_id", arrangement_option_id);
                yield return new KeyValuePair<string, object>(@"arrangement_options", arrangement_options);
            }
        }

        public RequestDetails request { get; set; }

        public int lease_id { get; set; }

        public int arrangement_option_id { get; set; }

        public ArrangementOption arrangement_options { get; set; }
    }
}
