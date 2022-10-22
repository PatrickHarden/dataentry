using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using dataentry.Data;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace dataentry.Repository
{
    public class AzureStorageRepository : IAzureStorageRepository
    {
        private readonly IConfiguration Configuration;

        public AzureStorageRepository(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<string> UploadBlobAsync(string blobName, string containerName, Stream stream)
        {
            //Blob
            BlockBlobClient blockBlob = await GetBlockBlobAsync(blobName, containerName);

            //Upload
            stream.Position = 0;
            await blockBlob.UploadAsync(stream);

            //Set Metadata
            await SetMetaData(blockBlob);

            return blockBlob.Uri.AbsoluteUri;
        }

        public async Task DeleteBlobAsync(string blobName, string containerName)
        {
            //Container
            BlobContainerClient blobContainer = await GetContainerAsync(containerName);

            //Blob
            var blob = blobContainer.GetBlockBlobClient(blobName);
            await blob.DeleteIfExistsAsync();
        }

        private static async Task SetMetaData(BlockBlobClient blockBlob)
        {
            if (blockBlob.BlobContainerName == ContainerEnum.photos.ToString())
            {
                await blockBlob.SetMetadataAsync(new Dictionary<string, string> {
                    { "HasMapping", "False" }
                });
            }
        }

        public async Task<bool> ExistsAsync(string blobName, string containerName)
        {
            //Blob
            BlockBlobClient blockBlob = await GetBlockBlobAsync(blobName, containerName);

            //Exists
            return await blockBlob.ExistsAsync();
        }

        private async Task<BlobContainerClient> GetContainerAsync(string containerName)
        {
            //Client
            BlobServiceClient blobClient = new BlobServiceClient(Configuration.GetConnectionString("BlobStorage"));

            //Container
            BlobContainerClient blobContainer = blobClient.GetBlobContainerClient(containerName);
            await blobContainer.CreateIfNotExistsAsync();

            return blobContainer;
        }

        private async Task<BlockBlobClient> GetBlockBlobAsync(string blobName, string containerName)
        {
            //Container
            BlobContainerClient blobContainer = await GetContainerAsync(containerName);

            //Blob
            BlockBlobClient blockBlob = blobContainer.GetBlockBlobClient(blobName);

            return blockBlob;
        }
    }
}