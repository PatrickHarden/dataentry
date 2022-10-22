using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class updateCompanyColloquialMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"updateCompanyColloquial"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"company_id", company_id);
                yield return new KeyValuePair<string, object>(@"colloquial_id", colloquial_id);
                yield return new KeyValuePair<string, object>(@"colloquial", colloquial);
            }
        }

        public RequestDetails request { get; set; }

        public int company_id { get; set; }

        public int colloquial_id { get; set; }

        public CompanyColloquialUpdate colloquial { get; set; }
    }
}
