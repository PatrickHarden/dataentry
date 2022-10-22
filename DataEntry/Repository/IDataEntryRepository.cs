using dataentry.Data.DBContext.Model;
using dataentry.Data.Enums;
using dataentry.ViewModels.GraphQL;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace dataentry.Repository
{
    public interface IDataEntryRepository
    {
        Task<IEnumerable<Listing>> GetAllListings(ClaimsPrincipal user, bool isAdmin, IEnumerable<FilterOption> filterOptions, string regionID, int? skip = null, int? take = null);
        Task<int> GetListingsCount(ClaimsPrincipal user, bool isAdmin, IEnumerable<FilterOption> filterOptions, string regionID);
        Task<Listing> GetListingByID(int ID, ClaimsPrincipal user);
        Task<IEnumerable<Listing>> GetEdpImportListings(ClaimsPrincipal user, int ID);
        Task<Listing> AddListing(Listing listing, ClaimsPrincipal claimsPrincipal, IEnumerable<string> userNames, IEnumerable<string> teamNames, bool ignoreDuplicateExternalId = true);
        Task<Listing> UpdateListing(Listing listing, ClaimsPrincipal user, IEnumerable<string> userNames = null, IEnumerable<string> teamNames = null);
        Task<IEnumerable<Region>> GetAllRegions();
        Task<bool> DeleteListing(int id, ClaimsPrincipal user);
        Task<IEnumerable<Broker>> GetAllBrokers();
        Task<Image> AddImage(Image image);
        Task<Image> UpdateImage(int id, int processStatus);
        Task<IEnumerable<Image>> GetImages(int? listingId, IEnumerable<int?> imageIds, ClaimsPrincipal user);
        Task<IEnumerable<Image>> GetImagesByWatermarkProcessState(int state, bool includeDeleted = true, int? imageType = null);
        Task<IEnumerable<Image>> ResetImagesWatermarkProcessState(int targetState, int finalState);
        Task<Broker> GetBrokerByEmail(string email);
        Task<IEnumerable<Broker>> GetBrokersByEmails(IEnumerable<string> emails);
        Task AddRegion(ClaimsPrincipal user, Region region);
        Task<Broker> GetBrokerById(int brokerId);
        Task<IEnumerable<Broker>> GetBrokers(IEnumerable<int> brokerIds);
        Task<Broker> AddOrUpdateBroker(Broker broker);
        Task<Region> GetRegionById(Guid id);
        Task<Region> GetRegionByHomeSiteId(string homeSiteId);
        Task<Region> GetDefaultRegion();
        Task UpdateRegion(ClaimsPrincipal user, Region region);
        Task<bool> DeleteRegion(ClaimsPrincipal user, Guid id);
        Task UpdateListingSerialization(int id, ListingSerializationType current, string value);
        Task<string> GetListingSerialization(int id, ListingSerializationType lastImport);
    }
}
