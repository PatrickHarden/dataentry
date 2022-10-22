using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using dataentry.ViewModels.GraphQL;

namespace dataentry.Services.Business.Regions
{
    public interface IRegionService
    {
        Task<RegionViewModel> GetRegionByHomeSiteId(string homeSiteId);
        Task<IEnumerable<RegionViewModel>> GetRegions();
        Task<RegionViewModel> CreateRegionAsync(ClaimsPrincipal user, RegionViewModel vm);
        Task<RegionViewModel> UpdateRegionAsync(ClaimsPrincipal user, RegionViewModel vm);
        Task<bool> DeleteRegionAsync(ClaimsPrincipal user, Guid id);
    }
}