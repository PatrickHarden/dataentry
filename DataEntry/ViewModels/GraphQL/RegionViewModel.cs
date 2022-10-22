using dataentry.AutoGraph.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.ViewModels.GraphQL
{
    [AutoObjectGraphType("Region")]
    [AutoInputObjectGraphType("RegionInput")]
    public class RegionViewModel
    {
        [Description("GUID ID")]
        public string ID { get; set; }
        [Description("User-friendly name of this region")]
        public string Name { get; set; }
        [Description("Example: us-comm")]
        public string HomeSiteID { get; set; }
        [Description("Example: us-comm-prev")]
        public string PreviewSiteID { get; set; }
        [Description("What prefix to use when generating an ID for a listing. Example: US-SMPL")]
        public string ListingPrefix { get; set; }
        [Description("What prefix to use when generating an ID for a listing preview. Example: US-PREV")]
        public string PreviewPrefix { get; set; }
        [Description("Example: en-US")]
        public string CultureCode { get; set; }
        [Description("Example: US")]
        public string CountryCode { get; set; }
        [Description("List of format strings to use when generating IDs")]
        public IEnumerable<RegionalIDFormatViewModel> RegionalIDFormats { get; set; }
    }
}
