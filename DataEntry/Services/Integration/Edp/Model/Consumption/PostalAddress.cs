using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Consumption
{   
    public class PostalAddress    {
        public string full_street_name { get; set; }
        public string street1 { get; set; } 
        public string street2 { get; set; } 
        public string city { get; set; } 
        public string state_province { get; set; } 
        public string postal_code { get; set; } 
        public string country { get; set; } 
        public decimal? latitude { get; set; } 
        public decimal? longitude { get; set; } 
    }
}