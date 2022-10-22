using System.Collections.Generic;
using dataentry.AutoGraph.Attributes;

namespace dataentry.ViewModels.GraphQL
{
    [AutoObjectGraphType("PointsOfInterests")]
    [AutoInputObjectGraphType("PointsOfInterestsInput")]
    public class PointsOfInterestsViewModel
    {
        [NonNull]
        public string InterestKind { get; set; }
        public IEnumerable<PlacesViewModel> Places { get; set; } 
    }
}
