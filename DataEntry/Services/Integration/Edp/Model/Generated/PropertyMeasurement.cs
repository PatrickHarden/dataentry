using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class PropertyMeasurement : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,property_element_type_desc,property_measurement_notes,property_measurement_size,property_measurement_size_uom}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public int? id { get; set; }

        [JsonProperty(@"property_element_type_desc", Required = Required.Always)]
        public string property_element_type_desc { get; set; }

        [JsonProperty(@"property_measurement_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string property_measurement_notes { get; set; }

        [JsonProperty(@"property_measurement_size", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? property_measurement_size { get; set; }

        [JsonProperty(@"property_measurement_size_uom", NullValueHandling = NullValueHandling.Ignore)]
        public string property_measurement_size_uom { get; set; }
    }
}
