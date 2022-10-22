using dataentry.Data.DBContext.Model;
using dataentry.ViewModels.GraphQL;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Business.Listings
{
    public interface IListingMapper
    {
        ListingViewModel Map(Listing listing);
        Listing Map(Listing listing, ListingViewModel listingViewModel);
        ContactsViewModel Map(Broker broker);
        Listing Map(Listing listing, SpacesViewModel spacesViewModel);
        IEnumerable<Tuple<Broker, int>> Map(IEnumerable<Broker> brokers, IEnumerable<ContactsViewModel> contacts);
        List<ListingBroker> Map(IEnumerable<ListingBroker> listingBrokers, IEnumerable<Tuple<Broker, int>> brokersWithOrder);
        Broker Map(Broker broker, ContactsViewModel contact);
    }
}
