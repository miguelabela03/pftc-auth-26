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

        public async Task<string> UploadFileAsync(IFormFile file, string fileNameForStorage)
        {
            if (file == null || file.Length == 0) 
            {
                throw new ArgumentNullException(nameof(file), "File is empty or null");
            }

            try
            {
                if (string.IsNullOrWhiteSpace(fileNameForStorage))
                {
                    fileNameForStorage = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                }

                string contentType = file.ContentType;
                var options = new UploadObjectOptions();

                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;

                    // Upload the file to Google Cloud Storage
                    var storageObject = await _storageClient.UploadObjectAsync(_bucketName, fileNameForStorage, contentType, memoryStream, options);

                    _logger.LogInformation($"Upload file {fileNameForStorage} to bucket {_bucketName}");

                    // Return URL
                    return $"https://storage.googleapis.com/{_bucketName}/{fileNameForStorage}";
                }

            } catch (Google.GoogleApiException gae)
            {
                _logger.LogError(gae, $"Google API Error during file upload: {gae.Message}");
                throw new ApplicationException($"Google API Error during file upload: {gae.Message}");
            }
            catch (Exception e)
            {
                _logger.LogError($"Unexpected error during file upload: {e.Message}");
                throw new ApplicationException($"Unexpected error during file upload: {e.Message}");
            }
        }
    }
}
