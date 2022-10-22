namespace dataentry.Data.DBContext.Model
{
    public class Address
    {
        public int ID { get; set; }
        public string PostalCode { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string StreetName { get; set; }
        public string StreetType { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string PreStreetDirectionName { get; set; }
        public string PostStreetDirectionName { get; set; }
        public string FullStreetName { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
