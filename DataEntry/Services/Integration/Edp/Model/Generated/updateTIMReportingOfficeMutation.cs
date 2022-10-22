using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class updateTIMReportingOfficeMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"updateTIMReportingOffice"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"tim_id", tim_id);
                yield return new KeyValuePair<string, object>(@"reporting_office_id", reporting_office_id);
            }
        }

        public RequestDetails request { get; set; }

        public int tim_id { get; set; }

        public int reporting_office_id { get; set; }
    }
}
