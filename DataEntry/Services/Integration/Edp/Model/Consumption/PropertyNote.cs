using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Consumption
{   
    public class PropertyNote
    {
        public int id { get; set; }
        public string property_notes_type_desc { get; set; }
        public string notes { get; set; }
        public List<Colloquial> colloquial { get; set; }
    }
    public class Colloquial
    {
        public string language_desc { get; set; }
        public string country_code_desc { get; set; }
        public string notes { get; set; }
        public string property_notes_type_desc { get; set; }
    }
}