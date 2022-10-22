using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using dataentry.Repository;
using dataentry.Services.Integration.Edp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace dataentry.Services.Business.Images
{
    public class FileService : IFileService
    {
        private readonly ILogger<FileService> _logger;
        private readonly AwsS3BucketOptions _configuration;
        private readonly string _awsKey;
        private readonly string _awsSecretKey;
        private readonly string _bucketName;
        private readonly string _bucketRegion;

        public FileService(ILogger<FileService> logger, IOptions<AwsS3BucketOptions> configuration)
        {
            _logger = logger;
            _configuration = configuration?.Value ?? throw new ArgumentNullException(nameof(configuration));
            _awsKey = _configuration.AwsKey;
            _awsSecretKey = _configuration.AwsSecretKey;
            _bucketName = _configuration.BucketName;
            _bucketRegion = _configuration.BucketRegion;

            if (string.IsNullOrWhiteSpace(_awsKey)) LogNullConfigWarning("AwsS3BucketOptions:AwsKey");
            if (string.IsNullOrWhiteSpace(_awsSecretKey)) LogNullConfigWarning("AwsS3BucketOptions:AwsSecretKey");
            if (string.IsNullOrWhiteSpace(_bucketName)) LogNullConfigWarning("AwsS3BucketOptions:BucketName");
            if (string.IsNullOrWhiteSpace(_bucketRegion)) LogNullConfigWarning("AwsS3BucketOptions:BucketRegion");
        }

        private void LogNullConfigWarning(string configKey)
        {
            _logger.LogWarning("Config value is null: {configKey}", configKey);
        }

        public async Task<string> GetFileBinary(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return null;
            using(_logger.BeginScope(new Dictionary<string, object> {["FileService_url"] = url}))
            {
                try
                {
                    Uri uri = new Uri(url);
                    RegionEndpoint endpoint = Amazon.RegionEndpoint.GetBySystemName(_bucketRegion);
                    var s3 = new AmazonS3Client(_awsKey, _awsSecretKey, endpoint);
                    string key = System.Web.HttpUtility.UrlDecode(uri.PathAndQuery?.Substring(uri.PathAndQuery.LastIndexOf(_bucketName) + _bucketName.Length + 1));

                    Byte[] bytes;
                    string contentType;
                    GetObjectRequest request= new GetObjectRequest()
                    {
                        BucketName = _bucketName,
                        Key = key
                    };
                    using (GetObjectResponse response = await s3.GetObjectAsync(request)) 
                    {
                        if (!((((int)response.HttpStatusCode) >= 200) && ((int)response.HttpStatusCode) <= 299)) {
                            throw new FileServiceException($"Error downloading file: Response code: {response.HttpStatusCode}");
                        }
                        using (Stream responseStream = response.ResponseStream)
                        using (MemoryStream filestream = new MemoryStream())
                        {
                            await response.ResponseStream.CopyToAsync(filestream);
                            contentType = response.Headers["Content-Type"];
                            bytes = new Byte[(int)filestream.Length];
                            bytes = filestream.ToArray();
                        }
                    }
                    if (bytes.Length == 0) throw new FileServiceException($"File content length was 0");
                    return $"data:{contentType};base64,{Convert.ToBase64String(bytes)}";
                }
                catch (AmazonS3Exception e)
                {
                    _logger.LogError($"Error encountered ***. Message:'{e.Message}' when reading object");
                    return null;
                }
                catch (Exception e)
                {
                    _logger.LogError($"Unknown encountered on server. Message:'{e.Message}' when reading object");
                    return null;
                }
            }
        }

        class FileServiceException : Exception
        {
            public FileServiceException(string message) : base(message)
            {
            }
        }
    }
}
