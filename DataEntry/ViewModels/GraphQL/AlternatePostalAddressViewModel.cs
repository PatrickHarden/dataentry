using dataentry.AutoGraph.Attributes;

namespace dataentry.ViewModels.GraphQL
{
    [AutoObjectGraphType("AlternatePostalAddress")]
    [AutoInputObjectGraphType("AlternatePostalAddressInput")]
    public class AlternatePostalAddressViewModel
    {
        public string Street { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string StateOrProvince { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
    }
}
