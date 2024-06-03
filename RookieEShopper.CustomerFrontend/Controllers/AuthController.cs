using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Xml;

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
            return RedirectToAction("Index","Home");
        }
    }
}
