using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Consumption
{    
    public class PropertyResult    {
        public Entity entity { get; set; } 
        public string property_name { get; set; } 
        public PostalAddress postal_address { get; set; } 
        public List<Usage> usages { get; set; } 
    }

    public class SearchProperty    {
        public int totalCount { get; set; } 
        public List<PropertyResult> properties { get; set; } 
    }

    public class Data    {
        public SearchProperty searchProperty { get; set; } 
    }

    public class PropertySearchResults    {
        public Data data { get; set; } 
    }
} 
