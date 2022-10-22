using dataentry.AutoGraph.Attributes;

namespace dataentry.ViewModels.GraphQL
{
    [AutoObjectGraphType("Configs")]
    [AutoInputObjectGraphType("ConfigsInput")]
    public class ConfigsViewModel
    {
        public string HomeSiteId { get; set; }
        public string AiKey { get; set; }
        public string PreviewFeatureFlag { get; set; } 
        public string WatermarkDetectionFeatureFlag { get; set; }
        public string MiqImportFeatureFlag { get; set; }
        public string MiqLimitSearchToCountryCodeFeatureFlag { get; set; }
        public string GoogleMapsKey { get; set; }
        public string GoogleMapsChannel { get; set; }
    }
}
