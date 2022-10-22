using dataentry.Data.DBContext.Model;
using dataentry.Repository;
using dataentry.Services.Business.Listings;
using dataentry.ViewModels.GraphQL;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dataentry.Services.Business.Contacts
{
    public class ContactService : IContactService
    {
        private readonly IListingMapper _listingMapper;
        private IDataEntryRepository _dataEntryRepository;

        public ContactService(
            IDataEntryRepository dataEntryRepository,
            IListingMapper listingMapper)
        {
            _dataEntryRepository = dataEntryRepository;
            _listingMapper = listingMapper;
        }

        public async Task<ContactsViewModel> SaveBroker(ContactsViewModel contact)
        {   
            var broker = await _dataEntryRepository.GetBrokerById(contact.ContactId);
            if (broker == null)
            {
                if (!string.IsNullOrWhiteSpace(contact.Email)) broker = await _dataEntryRepository.GetBrokerByEmail(contact.Email);
            }
            broker = _listingMapper.Map(broker, contact);
            var b = await _dataEntryRepository.AddOrUpdateBroker(broker);
            return _listingMapper.Map(b);
        }

        public async Task<IEnumerable<ContactsViewModel>> GetAllBrokers()
        {
            var query = await _dataEntryRepository.GetAllBrokers();
            return query.Select(b => _listingMapper.Map(b));
        }
    }
}
