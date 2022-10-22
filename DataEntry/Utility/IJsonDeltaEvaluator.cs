using dataentry.ViewModels.GraphQL;
using System.Collections.Generic;

namespace dataentry.Utility
{
    public interface IJsonDeltaEvaluator
    {
        string Apply(string originalDocument, IEnumerable<ListingDeltaViewModel> deltas);
        IEnumerable<ListingDeltaViewModel> Evaluate(string originalDocument, string newDocument);
    }
}