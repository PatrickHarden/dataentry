using dataentry.AutoGraph.Attributes;

namespace dataentry.ViewModels.GraphQL
{
    [AutoObjectGraphType("ChargesAndModifiers")]
    [AutoInputObjectGraphType("ChargesAndModifiersInput")]
    public class ChargesAndModifiersViewModel
    {
        public string ChargeType { get; set; }
        public string ChargeModifier { get; set; }
        public string Term { get; set; }
        public decimal? Amount { get; set; }
        public string PerUnitType { get; set; }
        public string CurrencyCode { get; set; }
        public int? Year { get; set; }
    }
}
