using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class updateLeasePropertyMeasurementMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"updateLeasePropertyMeasurement"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"lease_id", lease_id);
                yield return new KeyValuePair<string, object>(@"property_measurement_arrangement_id", property_measurement_arrangement_id);
                yield return new KeyValuePair<string, object>(@"property_measurement_arrangement", property_measurement_arrangement);
            }
        }

        public RequestDetails request { get; set; }

        public int lease_id { get; set; }

        public int property_measurement_arrangement_id { get; set; }

        public PropertyMeasurementUpdate property_measurement_arrangement { get; set; }
    }
}
