using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Consumption
{   
     public class PropertyMeasurement
    {
        public int id { get; set; }
        public decimal property_measurement_size { get; set; }
        public string property_element_type_desc { get; set; }
        public string property_measurement_size_uom { get; set; }
    }
}