using System.Collections.Generic;
using dataentry.AutoGraph.Attributes;

namespace dataentry.ViewModels.GraphQL
{
    [AutoObjectGraphType("TransportationTypes")]
    [AutoInputObjectGraphType("TransportationTypesInput")]
    public class TransportationTypesViewModel
    {
        [NonNull]
        public string Type { get; set; }
        public IEnumerable<PlacesViewModel> Places { get; set; } 
    }
}
