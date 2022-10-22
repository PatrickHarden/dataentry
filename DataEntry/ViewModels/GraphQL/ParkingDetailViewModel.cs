using System.Collections.Generic;
using dataentry.AutoGraph.Attributes;

namespace dataentry.ViewModels.GraphQL
{
    [AutoObjectGraphType("ParkingDetail")]
    [AutoInputObjectGraphType("ParkingDetailInput")]
    public class ParkingDetailViewModel
    {
        public string ParkingType { get; set; }
        public double? ParkingSpace { get; set; }
        public double? Amount { get; set; }
        public string Interval { get; set; }
        public string CurrencyCode { get; set; }
    }
}
