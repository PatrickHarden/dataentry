using dataentry.AutoGraph.Attributes;

namespace dataentry.ViewModels.GraphQL
{
    [AutoObjectGraphType("AdditionalFields")]
    [AutoInputObjectGraphType("AdditionalFieldsInput")]
    public class AdditionalFieldsViewModel
    {
        public string License { get; set; }
    }
}
