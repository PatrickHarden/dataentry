using dataentry.AutoGraph.Attributes;
using System;

namespace dataentry.ViewModels.GraphQL
{
    [AutoObjectGraphType("State")]
    public class ListingStateViewModel
    {
        public string PublishState { get; set; }
        public DateTimeOffset PublishStateDateUpdated { get; set; }
    }
}
