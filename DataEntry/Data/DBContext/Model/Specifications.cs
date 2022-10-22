namespace dataentry.Data.DBContext.Model
{
    public class Specifications
    {
        public string LeaseType { get; set; }
        public string Measure { get; set; }
        public string LeaseTerm { get; set; }
        public decimal? MinSpace { get; set; }
        public decimal? MaxSpace { get; set; }
        public decimal? TotalSpace { get; set; }
        public decimal? MaxPrice { get; set; }
        public decimal? MinPrice { get; set; }
        public string TaxModifer { get; set; }
        public bool ContactBrokerForPrice { get; set; }
        public int? Bedrooms { get; set; }
        public string CurrencyCode { get; set; }
        public string LeaseRateType { get; set; }
        public decimal? SalePrice { get; set; }
        public bool? ShowPriceWithUoM { get; set; }
        public bool? AutoCalculateMinSpace { get; set; }
        public bool? AutoCalculateTotalSpace { get; set; }
        public bool? AutoCalculateMinPrice { get; set; }
        public bool? AutoCalculateMaxPrice { get; set; }
        public bool? AutoCalculateTotalPrice { get; set; }
    }
}
