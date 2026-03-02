using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pftc_auth.DataAccess;
using pftc_auth.Models;

namespace pftc_auth.Controllers
{
    public class SocialController : Controller
    {
        private FirestoreRepository _repo;
        private ILogger<SocialController> _logger;
        public SocialController(ILogger<SocialController> logger, FirestoreRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var posts = await _repo.GetPosts();
            return View(posts);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePost(SocialMediaPost p)
        {
            p.PostId = Guid.NewGuid().ToString();
            p.PostAuthor = User.Identity.Name;
            p.PostDate = DateTimeOffset.UtcNow;
            await _repo.CreatePost(p);
            return RedirectToAction("Index", controllerName:"Social");
        }

        // Delete
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeletePost(string postId)
        {
            // 1. Validation: Ensure we actually received an ID
            if (string.IsNullOrWhiteSpace(postId))
            {
                return BadRequest(new { error = "Post ID is required." });
            }

            try
            {
                // 2. Optional: Verify the post exists before attempting deletion
                SocialMediaPost post = await _repo.GetPostById(postId);

                if (post.PostAuthor != User.Identity.Name)
                {
                    _logger.LogError($"User {User.Identity.Name} tried to delete a post that does not belong to him! " +
                        $"It actually belongs to {post.PostAuthor}", postId);
                    return Forbid("You can only delete your own post...");
                }

                // 3. Call the repository to remove it from Firestore
                await _repo.DeletePost(postId);

                // 4. Return a JSON success response for AJAX or a redirect for standard forms
                return Ok(new { success = "true", message = "Post Deleted" });
            }
            catch (KeyNotFoundException e)
            {
                // Log the specific missing ID error
                _logger.LogError($"{postId} not Found: {e.Message}");
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                // Catch-all for database connection issues or permissions
                _logger.LogError($"Error deleting post {postId}: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        // Update
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdatePost()
        {
            // await _repo.UpdatePost();
            return RedirectToAction("Index", "Social");
        }
    }
}
