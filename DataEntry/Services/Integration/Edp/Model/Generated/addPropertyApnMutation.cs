using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class addPropertyApnMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"addPropertyApn"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"property_id", property_id);
                yield return new KeyValuePair<string, object>(@"apn", apn);
            }
        }

        public RequestDetails request { get; set; }

        public int property_id { get; set; }

        public PropertyApnNumber apn { get; set; }
    }
}
