using System.Collections.Generic;
namespace dataentry.Data.DBContext.Model
{
    public class Parkings
    {
        public double? Ratio { get; set; }
        public double? RatioPer { get; set; }
        public string RatioPerUnit { get; set; }
        public List<ParkingDetail> ParkingDetails { get; set; } 
    }
}
