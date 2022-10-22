using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class updateContactMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"updateContact"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"contact_id", contact_id);
                yield return new KeyValuePair<string, object>(@"contact", contact);
            }
        }

        public RequestDetails request { get; set; }

        public int contact_id { get; set; }

        public ContactUpdate contact { get; set; }
    }
}
