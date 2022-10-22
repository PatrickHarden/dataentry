using System;
using dataentry.AutoGraph.Attributes;

namespace dataentry.ViewModels.GraphQL
{
    [AutoObjectGraphType("ImageDetection")]
    [AutoInputObjectGraphType("ImageDetectionInput")]
    public class ImageDetectionViewModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int WatermarkProcessStatus { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
