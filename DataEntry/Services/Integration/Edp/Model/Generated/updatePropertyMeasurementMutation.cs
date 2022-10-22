using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class updatePropertyMeasurementMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"updatePropertyMeasurement"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"property_id", property_id);
                yield return new KeyValuePair<string, object>(@"property_measurement_id", property_measurement_id);
                yield return new KeyValuePair<string, object>(@"property_measurement", property_measurement);
            }
        }

        public RequestDetails request { get; set; }

        public int property_id { get; set; }

        public int property_measurement_id { get; set; }

        public PropertyMeasurementUpdate property_measurement { get; set; }
    }
}
