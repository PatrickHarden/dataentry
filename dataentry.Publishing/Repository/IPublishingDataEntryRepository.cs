using dataentry.Publishing.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace dataentry.Publishing.Repository
{
    public interface IPublishingDataEntryRepository
    {
        Task<IEnumerable<PublishListing>> GetPublishListings(PublishState publishState);
        Task UpdatePublishListingState(int ListingId, PublishState publishState);
    }
}
