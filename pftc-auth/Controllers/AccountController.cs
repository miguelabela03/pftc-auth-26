using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;

namespace pftc_auth.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            string? redirectUrl = Url.Action("GoogleResponse", controller: "Account");
            AuthenticationProperties properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, authenticationSchemes: GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync();

            if (!result.Succeeded)
            {
                return RedirectToAction("Login");
            }

            return RedirectToAction("Index", controllerName: "Home");
        }

        public IActionResult Logout()
        {
            return SignOut(authenticationSchemes: "Cookies");
        }
    }
}
