using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class updateTIMMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"updateTIM"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"tim_id", tim_id);
                yield return new KeyValuePair<string, object>(@"tim", tim);
            }
        }

        public RequestDetails request { get; set; }

        public int tim_id { get; set; }

        public TIMUpdate tim { get; set; }
    }
}
