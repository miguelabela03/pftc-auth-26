using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using pftc_auth.Models;

namespace pftc_auth.DataAccess
{
    public class FirestoreRepository
    {
        private readonly ILogger<FirestoreRepository> _logger;
        private FirestoreDb _db;

        public FirestoreRepository(ILogger<FirestoreRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _db = FirestoreDb.Create(configuration["Authentication:Google:ProjectId"]);
        }

        public async Task CreatePost(SocialMediaPost p)
        {
            await _db.Collection("post").AddAsync(p);
            _logger.LogInformation($"Post by {p.PostAuthor} created successfully.");
        }
    }
}
