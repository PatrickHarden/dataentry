using System;
using System.Collections.Generic;
using System.Linq;
using dataentry.Data.DBContext.Model;
using dataentry.ViewModels.GraphQL;

namespace dataentry.Services.Business.Regions
{
    public class RegionMapper : IRegionMapper
    {
        public RegionViewModel Map(Region region)
        {
            var vm = new RegionViewModel();

            vm.ID = region.ID.ToString();
            vm.Name = region.Name;
            vm.HomeSiteID = region.HomeSiteID;
            vm.PreviewSiteID = region.PreviewSiteID;
            vm.ListingPrefix = region.ListingPrefix;
            vm.PreviewPrefix = region.PreviewPrefix;
            vm.CultureCode = region.CultureCode;
            vm.CountryCode = region.CountryCode;
            vm.RegionalIDFormats = region.RegionalIDFormats?.Select(Map);

            return vm;
        }

        private RegionalIDFormatViewModel Map(RegionalIDFormat format)
        {
            var vm = new RegionalIDFormatViewModel();
            
            vm.SourceSystemName = format.SourceSystemName;
            vm.FormatString = format.FormatString;

            return vm;
        }

        public void Map(Region region, RegionViewModel vm)
        {
            region.Name = vm.Name;
            region.HomeSiteID = vm.HomeSiteID;
            region.PreviewSiteID = vm.PreviewSiteID;
            region.ListingPrefix = vm.ListingPrefix;
            region.PreviewPrefix = vm.PreviewPrefix;
            region.CultureCode = vm.CultureCode;
            region.CountryCode = vm.CountryCode;

            var touchedFormats = new HashSet<RegionalIDFormat>();
            if (vm.RegionalIDFormats != null) {
                foreach (var vmFormat in vm.RegionalIDFormats)
                {
                    // Update or create
                    var format = region.RegionalIDFormats.FirstOrDefault(f => f.SourceSystemName == vmFormat.SourceSystemName);
                    if (format == null) {
                        format = new RegionalIDFormat();
                        region.RegionalIDFormats.Add(format);
                    }

                    // Whitelist of formats to not delete afterwards
                    touchedFormats.Add(format);
                    
                    format.SourceSystemName = vmFormat.SourceSystemName;
                    format.FormatString = vmFormat.FormatString;
                }

                region.RegionalIDFormats.RemoveAll(f => !touchedFormats.Contains(f));
            }
        }
    }
}
