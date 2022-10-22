using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dataentry.ViewModels.GraphQL;

namespace dataentry.Services.Integration.Edp.Consumption
{
    public interface IConsumptionService
    {
        bool Enabled { get; }
        Task<IEnumerable<PropertySearchResultViewModel>> SearchProperties(string keyword, string country);
        Task<ListingViewModel> GetListingByMiqId(int id, Guid regionId, bool serializeFileData);
        Task<ListingViewModel> ConvertPropertyWithAvailabilityToListingViewModel(PropertyWithAvailability property, Guid regionId);
        Task<PropertyWithAvailability> GetPropertyWithAvailability(int id, Guid regionId);
    }
}
