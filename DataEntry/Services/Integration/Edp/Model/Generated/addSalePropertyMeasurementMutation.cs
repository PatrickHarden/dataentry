using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class addSalePropertyMeasurementMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"addSalePropertyMeasurement"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"sale_id", sale_id);
                yield return new KeyValuePair<string, object>(@"property_measurement_arrangement", property_measurement_arrangement);
            }
        }

        public RequestDetails request { get; set; }

        public int sale_id { get; set; }

        public PropertyMeasurement property_measurement_arrangement { get; set; }
    }
}
