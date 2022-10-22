using dataentry.AutoGraph.Attributes;

namespace dataentry.ViewModels.GraphQL
{
    [AutoObjectGraphType("RegionalIDFormat")]
    [AutoInputObjectGraphType("RegionalIDFormatInput")]
    public class RegionalIDFormatViewModel
    {
        [Description("The original system this property originated from as it appears in MarketIQ")]
        public string SourceSystemName { get; set; }
        
        [Description("String formatter used to generate an external id. {0} represents the original system's ID found in MarketIQ's source_lineage field")]
        public string FormatString { get; set; }
    }
}