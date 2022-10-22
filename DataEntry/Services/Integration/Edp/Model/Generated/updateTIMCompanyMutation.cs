using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class updateTIMCompanyMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"updateTIMCompany"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"tim_id", tim_id);
                yield return new KeyValuePair<string, object>(@"company_id", company_id);
            }
        }

        public RequestDetails request { get; set; }

        public int tim_id { get; set; }

        public int company_id { get; set; }
    }
}
