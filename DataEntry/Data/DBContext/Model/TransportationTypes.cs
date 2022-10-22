using System.Collections.Generic;
namespace dataentry.Data.DBContext.Model
{
    public class TransportationTypes
    {
        public string Type { get; set; }
        public List<Place> Places { get; set; }
    }
}
