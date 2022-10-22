using System.Collections.Generic;
namespace dataentry.Data.DBContext.Model
{
    public class PointsOfInterests
    {
        public string InterestKind { get; set; }
        public List<Place> Places { get; set; }
    }
}
