using dataentry.AutoGraph.Attributes;

namespace dataentry.ViewModels.GraphQL
{
    [AutoObjectGraphType("ListingDelta")]
    [AutoInputObjectGraphType("ListingDeltaInput")]
    public class ListingDeltaViewModel
    {
        public string OriginalDocumentPath { get; set; }
        public string NewDocumentPath { get; set; }
        public string OriginalValue { get; set; }
        public string NewValue { get; set; }
        public string JsonPath { get; set; }
    }
}
