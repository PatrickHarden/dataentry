using dataentry.Utility;

namespace dataentry.Services.Integration.Edp
{
    public class EdpOptions
    {
        public bool Enabled { get; set; }
        public string Endpoint { get; set; }
        public string SourceSystemName { get; set; }
        public string SourceSubmitterName { get; set; }
        public string UserRole { get; set; }
        public CaseInsensitiveDictionary<bool> RegionOverrides { get; set; }

        public bool EnabledInRegion(string homeSiteID)
        {
            if (RegionOverrides == null) return Enabled;
            if (string.IsNullOrWhiteSpace(homeSiteID)) return Enabled;
            homeSiteID = homeSiteID.Replace('-', '_');
            return RegionOverrides.TryGetValue(homeSiteID, out var enabledInRegion) ? enabledInRegion : Enabled;
        }
    }
}