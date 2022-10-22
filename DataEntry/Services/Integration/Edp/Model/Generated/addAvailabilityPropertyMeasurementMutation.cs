using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class addAvailabilityPropertyMeasurementMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"addAvailabilityPropertyMeasurement"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"availability_id", availability_id);
                yield return new KeyValuePair<string, object>(@"property_measurement_availability", property_measurement_availability);
            }
        }

        public RequestDetails request { get; set; }

        public int availability_id { get; set; }

        public PropertyMeasurement property_measurement_availability { get; set; }
    }
}
