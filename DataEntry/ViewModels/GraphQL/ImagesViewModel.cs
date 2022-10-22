using dataentry.AutoGraph.Attributes;

namespace dataentry.ViewModels.GraphQL
{
    [AutoObjectGraphType("Images")]
    [AutoInputObjectGraphType("ImagesInput")]
    public class ImagesViewModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string DisplayText { get; set; }
        public bool Active { get; set; }
        public bool Primary { get; set; }
        public bool Watermark { get; set; }
        public bool UserOverride { get; set; }
        public int Order { get; set; }
        public int WatermarkProcessStatus { get; set; }
        public string Base64String { get; set; }
    }
}
