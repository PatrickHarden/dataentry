using dataentry.AutoGraph.Attributes;

namespace dataentry.ViewModels.GraphQL
{
    [AutoObjectGraphType("Media")]
    [AutoInputObjectGraphType("MediaInput")]
    public class MediaViewModel
    {
        [NonNull]
        public string Url { get; set; }

        [NonNull]
        public string DisplayText { get; set; }

        [NonNull]
        public bool Active { get; set; }

        [NonNull]
        public bool Primary { get; set; }

        public string Base64String { get; set; }
    }
}