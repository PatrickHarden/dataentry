using System.Threading.Tasks;
using dataentry.Services.Integration.Edp.Model;
using Microsoft.Extensions.Logging;

namespace dataentry.Services.Integration.Edp
{
    public interface IEdpGraphQLService
    {
        Task<string> GetApiVersionAsync();
        Task<string> RunQueryRaw<T>(EdpGraphQLQuery<T> query) where T: EdpGraphQLObject, new();
        Task<T> RunQuery<T>(EdpGraphQLQuery<T> query) where T: EdpGraphQLObject, new();
    }
}
