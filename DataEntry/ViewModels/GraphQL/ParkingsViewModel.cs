using System.Collections.Generic;
using dataentry.AutoGraph.Attributes;

namespace dataentry.ViewModels.GraphQL
{
    [AutoObjectGraphType("Parkings")]
    [AutoInputObjectGraphType("ParkingsInput")]
    public class ParkingsViewModel
    {
        public double? Ratio { get; set; }
        public double? RatioPer { get; set; }
        public string RatioPerUnit { get; set; }
        public IEnumerable<ParkingDetailViewModel> ParkingDetails { get; set; } 
    }
}
