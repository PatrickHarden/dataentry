using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class deletePropertyApnMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"deletePropertyApn"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"property_id", property_id);
                yield return new KeyValuePair<string, object>(@"apn_id", apn_id);
            }
        }

        public RequestDetails request { get; set; }

        public int property_id { get; set; }

        public int apn_id { get; set; }
    }
}
