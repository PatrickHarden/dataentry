using dataentry.Data.DBContext.Model;
using dataentry.ViewModels.GraphQL;

namespace dataentry.Services.Business.Regions
{
    public interface IRegionMapper
    {
        RegionViewModel Map(Region region);
        void Map(Region region, RegionViewModel vm);
    }
}