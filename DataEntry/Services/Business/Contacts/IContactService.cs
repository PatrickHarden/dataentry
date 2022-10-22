using dataentry.ViewModels.GraphQL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dataentry.Services.Business.Contacts
{
    public interface IContactService
    {
        Task<ContactsViewModel> SaveBroker(ContactsViewModel contact);
        Task<IEnumerable<ContactsViewModel>> GetAllBrokers();
    }
}
