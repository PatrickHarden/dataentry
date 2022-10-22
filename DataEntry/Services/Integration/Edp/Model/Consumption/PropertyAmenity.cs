using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Consumption
{
    public class PropertyAmenity
    {
        public int id { get; set; }
        public string amenity_name { get; set; }
        public string property_amenity_type_desc { get; set; }
        public string amenity_type_desc { get; set; }
        public string amenity_subtype_desc { get; set; }
        public string amenity_notes { get; set; }
        public IEnumerable<PropertyAmenityColl> colloquial { get; set; }
    }
}