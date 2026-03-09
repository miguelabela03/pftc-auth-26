using Google.Cloud.Storage.V1;
using pftc_auth.Interfaces;

namespace pftc_auth.Services
{
    public class BucketStorageService: IBucketStorageService
    {
        private readonly ILogger<BucketStorageService> _logger;
        private readonly string _bucketName;
        private readonly StorageClient _storageClient;

        public BucketStorageService(ILogger<BucketStorageService> logger, IConfiguration config)
        {
            _logger = logger;
            _bucketName = config.GetValue<string>("Storage:Google:BucketName");
            _storageClient = StorageClient.Create();
        }

        public Task DeleteFileAsync(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<string> UploadFileAsync(IFormFile file, string fileNameForStorage)
        {
            throw new NotImplementedException();
        }
    }
}
