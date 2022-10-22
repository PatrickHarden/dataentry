using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class updateReviewMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"updateReview"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"edp_request_id", edp_request_id);
                yield return new KeyValuePair<string, object>(@"entity", entity);
                yield return new KeyValuePair<string, object>(@"attribute_name", attribute_name);
                yield return new KeyValuePair<string, object>(@"reviewed_by", reviewed_by);
                yield return new KeyValuePair<string, object>(@"reviewed_source", reviewed_source);
                yield return new KeyValuePair<string, object>(@"reviewed_status", reviewed_status);
                yield return new KeyValuePair<string, object>(@"comment", comment);
                yield return new KeyValuePair<string, object>(@"user_role", user_role);
            }
        }

        public string edp_request_id { get; set; }

        public string entity { get; set; }

        public string attribute_name { get; set; }

        public string reviewed_by { get; set; }

        public string reviewed_source { get; set; }

        public string reviewed_status { get; set; }

        public string comment { get; set; }

        public string user_role { get; set; }
    }
}
