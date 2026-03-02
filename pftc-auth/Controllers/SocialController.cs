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
        public IActionResult Index()
        {
            return View();
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
    }
}
