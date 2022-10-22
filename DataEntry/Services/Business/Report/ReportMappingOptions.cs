using System.Collections.Generic;

namespace dataentry.Services.Business.Report
{
    public class ReportMappingOptions
    {
        public string DefaultUrl { get; set; }
        public Dictionary<string, string> Mapping { get; set; }
    }
}
