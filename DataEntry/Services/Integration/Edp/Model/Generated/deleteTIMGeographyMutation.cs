using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class deleteTIMGeographyMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"deleteTIMGeography"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"tim_id", tim_id);
                yield return new KeyValuePair<string, object>(@"current_geography", current_geography);
                yield return new KeyValuePair<string, object>(@"target_geography", target_geography);
            }
        }

        public RequestDetails request { get; set; }

        public int tim_id { get; set; }

        public TIMGeography current_geography { get; set; }

        public TIMGeography target_geography { get; set; }
    }
}
