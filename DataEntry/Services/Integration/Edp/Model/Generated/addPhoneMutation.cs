using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class addPhoneMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"addPhone"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"entity", entity);
                yield return new KeyValuePair<string, object>(@"entity_id", entity_id);
                yield return new KeyValuePair<string, object>(@"phone", phone);
                yield return new KeyValuePair<string, object>(@"location_id", location_id);
            }
        }

        public RequestDetails request { get; set; }

        public string entity { get; set; }

        public int entity_id { get; set; }

        public Phone phone { get; set; }

        public int? location_id { get; set; }
    }
}
