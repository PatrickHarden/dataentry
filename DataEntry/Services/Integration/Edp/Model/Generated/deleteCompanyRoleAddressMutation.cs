using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class deleteCompanyRoleAddressMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"deleteCompanyRoleAddress"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"contact_id", contact_id);
                yield return new KeyValuePair<string, object>(@"company_role_address_id", company_role_address_id);
                yield return new KeyValuePair<string, object>(@"expiry_date", expiry_date);
            }
        }

        public RequestDetails request { get; set; }

        public int contact_id { get; set; }

        public int company_role_address_id { get; set; }

        public DateTime? expiry_date { get; set; }
    }
}
