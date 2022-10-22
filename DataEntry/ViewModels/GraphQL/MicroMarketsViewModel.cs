using dataentry.AutoGraph.Attributes;

namespace dataentry.ViewModels.GraphQL
{
    [AutoObjectGraphType("MicroMarkets")]
    [AutoInputObjectGraphType("MicroMarketsInput")]
    public class MicroMarketsViewModel
    {
        [NonNull]
        public string Value { get; set; }
        public int Order { get; set; }
    }
}
