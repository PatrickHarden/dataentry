using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class deleteBrokerLicenseMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"deleteBrokerLicense"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"contact_id", contact_id);
                yield return new KeyValuePair<string, object>(@"license_id", license_id);
            }
        }

        public RequestDetails request { get; set; }

        public int contact_id { get; set; }

        public int license_id { get; set; }
    }
}
