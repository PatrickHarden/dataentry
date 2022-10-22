using System.Threading.Tasks;

namespace dataentry.Services.Business.Users
{
    public interface IUserLoginService
    {
        Task<string> Authenticate(string username, string password, string nonce, string clientId);
    }
}
