using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class addPropertyMediaInformationMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"addPropertyMediaInformation"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"property_id", property_id);
                yield return new KeyValuePair<string, object>(@"property_media_information", property_media_information);
            }
        }

        public RequestDetails request { get; set; }

        public int property_id { get; set; }

        public PropertyMediaInformation property_media_information { get; set; }
    }
}
