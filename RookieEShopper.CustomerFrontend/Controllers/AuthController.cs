using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RookieEShopper.CustomerFrontend.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Logout()
        {
            return SignOut("Cookie", "oidc");
        }

        [Authorize]
        public IActionResult Login()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}