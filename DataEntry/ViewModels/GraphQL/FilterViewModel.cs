using dataentry.AutoGraph.Attributes;

namespace dataentry.ViewModels.GraphQL
{
    [AutoObjectGraphType("Filter")]
    [AutoInputObjectGraphType("FilterInput")]
    public class FilterViewModel
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
