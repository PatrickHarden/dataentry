using System.Collections.Generic;
using dataentry.Services.Integration.Edp.Model;

namespace dataentry.Services.Integration.Edp.Consumption
{   
    public class PropertyWithAvailability    
    {
        public PropertyDetail PropertyDetail { get; set; }
        public List<Availability> Availability { get; set; }
    }
} 
