using System.Collections.Generic;
using dataentry.AutoGraph.Attributes;

namespace dataentry.ViewModels.GraphQL
{
    [AutoObjectGraphType("ImportListing")]
    public class ImportListingViewModel
    {
        public IEnumerable<ImportListingDetailViewModel> Listings { get; set; }
        public string ErrorMessage { get; set; }
    }

    [AutoObjectGraphType("ImportListingDetail")]
    public class ImportListingDetailViewModel
    {
        public int Id { get; set; }
        public string ExternalId { get; set; }
        public string ErrorMessage { get; set; }
    }
}
