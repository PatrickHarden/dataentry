using dataentry.AutoGraph.Attributes;

namespace dataentry.ViewModels.GraphQL
{
    [AutoObjectGraphType("Places")]
    [AutoInputObjectGraphType("PlacesInput")]
    public class PlacesViewModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal? Distances { get; set; }
        public string DistanceUnits { get; set; }
        public decimal? Duration { get; set; }
        public string TravelMode { get; set; }
        public int? Order { get; set; }
    }
}