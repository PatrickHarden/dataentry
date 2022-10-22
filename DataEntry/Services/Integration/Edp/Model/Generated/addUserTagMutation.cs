using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class addUserTagMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"addUserTag"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"entity", entity);
                yield return new KeyValuePair<string, object>(@"entity_id", entity_id);
                yield return new KeyValuePair<string, object>(@"user_tags", user_tags);
            }
        }

        public RequestDetails request { get; set; }

        public string entity { get; set; }

        public int entity_id { get; set; }

        public List<UserTag> user_tags { get; set; }
    }
}
