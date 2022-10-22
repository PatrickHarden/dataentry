namespace dataentry.Data.DBContext.Model
{
    public class ParkingDetail
    {
        public string ParkingType { get; set; }
        public double? ParkingSpace { get; set; }
        public double? Amount { get; set; }
        public string Interval { get; set; }
        public string CurrencyCode { get; set; }
    }
}