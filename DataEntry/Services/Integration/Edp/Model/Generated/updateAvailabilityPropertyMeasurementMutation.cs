using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class updateAvailabilityPropertyMeasurementMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"updateAvailabilityPropertyMeasurement"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"availability_id", availability_id);
                yield return new KeyValuePair<string, object>(@"property_measurement_availability_id", property_measurement_availability_id);
                yield return new KeyValuePair<string, object>(@"property_measurement_availability", property_measurement_availability);
            }
        }

        public RequestDetails request { get; set; }

        public int availability_id { get; set; }

        public int property_measurement_availability_id { get; set; }

        public PropertyMeasurementUpdate property_measurement_availability { get; set; }
    }
}
