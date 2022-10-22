namespace dataentry.Data.DBContext.Model
{
    public class Place
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal? Distances { get; set; }
        public string DistanceUnits { get; set; }
        public decimal? Duration { get; set; }
        public string TravelMode { get; set; }
        public int? Order { get; set; }
    }
}