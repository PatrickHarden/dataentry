using dataentry.AutoGraph.Attributes;

namespace dataentry.ViewModels.GraphQL
{
    [AutoObjectGraphType("PropertySizes")]
    [AutoInputObjectGraphType("PropertySizesInput")]
    public class PropertySizesViewModel
    {
        public string SizeKind { get; set; }
        public string MeasureUnit { get; set; }
        public double Amount { get; set; }
    }
}
