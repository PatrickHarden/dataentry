using System.IO;
using System.Threading.Tasks;

namespace dataentry.Repository
{
    public interface IAzureStorageRepository
    {
        Task<string> UploadBlobAsync(string blobName,string containerName, Stream stream);
        Task<bool> ExistsAsync(string blobName, string containerName);
        Task DeleteBlobAsync(string blobName, string containerName);
    }
}
