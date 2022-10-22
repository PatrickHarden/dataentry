using dataentry.AutoGraph.Attributes;

namespace dataentry.ViewModels.GraphQL
{
    [AutoObjectGraphType("Highlight")]
    [AutoInputObjectGraphType("HighlightInput")]
    [Description("A short description of a marketable feature of a listing.")]
    public class HighlightViewModel : OrderedTextTypeViewModel
    {
        [Description("The ID of the usage amenity in MIQ that this highlight was generated from during an import.")]
        public string MiqId { get; set; }
    }
}
