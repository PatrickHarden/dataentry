using dataentry.Data.DBContext.Model;
using dataentry.Repository;
using dataentry.ViewModels.GraphQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace dataentry.Services.Business.Regions
{
    public class RegionService : IRegionService
    {
        private readonly IDataEntryRepository _dataEntryRepository;
        private readonly IRegionMapper _regionMapper;

        public RegionService(IDataEntryRepository dataEntryRepository, IRegionMapper regionMapper)
        {
            _dataEntryRepository = dataEntryRepository ?? throw new ArgumentNullException(nameof(dataEntryRepository));
            _regionMapper = regionMapper ?? throw new ArgumentNullException(nameof(regionMapper));
        }

        public async Task<IEnumerable<RegionViewModel>> GetRegions()
        {
            var regions = await _dataEntryRepository.GetAllRegions();
            return regions.Select(region => _regionMapper.Map(region));
        }

        public async Task<RegionViewModel> GetRegionByHomeSiteId(string homeSiteId)
        {
            var region = await _dataEntryRepository.GetRegionByHomeSiteId(homeSiteId);
            return _regionMapper.Map(region);
        }

        public async Task<RegionViewModel> CreateRegionAsync(ClaimsPrincipal user, RegionViewModel vm)
        {
            var region = new Region();
            _regionMapper.Map(region, vm);
            await _dataEntryRepository.AddRegion(user, region);
            vm.ID = region.ID.ToString();
            return vm;
        }

        public async Task<RegionViewModel> UpdateRegionAsync(ClaimsPrincipal user, RegionViewModel vm)
        {
            var region = await _dataEntryRepository.GetRegionById(new Guid(vm.ID));
            _regionMapper.Map(region, vm);
            await _dataEntryRepository.UpdateRegion(user, region);
            return vm;
        }

        public async Task<bool> DeleteRegionAsync(ClaimsPrincipal user, Guid id)
        {
            return await _dataEntryRepository.DeleteRegion(user, id);
        }
    }
}
