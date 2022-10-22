using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class deleteTenantPropertySpaceMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"deleteTenantPropertySpace"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"tenant_id", tenant_id);
                yield return new KeyValuePair<string, object>(@"property_space_id", property_space_id);
            }
        }

        public RequestDetails request { get; set; }

        public int tenant_id { get; set; }

        public List<int> property_space_id { get; set; }
    }
}
