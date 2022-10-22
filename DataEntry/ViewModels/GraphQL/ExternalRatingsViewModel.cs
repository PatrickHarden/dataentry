using dataentry.AutoGraph.Attributes;

namespace dataentry.ViewModels.GraphQL
{
    [AutoObjectGraphType("ExternalRatings")]
    [AutoInputObjectGraphType("ExternalRatingsInput")]
    public class ExternalRatingsViewModel
    {
        public string RatingType { get; set; }
        public string RatingLevel { get; set; }
    }
}