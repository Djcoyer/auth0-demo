using Microsoft.Owin.Security.Cookies;
using MvcApplication.Models;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Linq;
using MvcApplication.ViewModels;

namespace MvcApplication.Controllers
{
    public class AccountController : Controller
    {

        public ActionResult Login(string returnUrl)
        {
            return new ChallengeResult("Auth0", returnUrl ?? Url.Action("Index", "Home"));
        }

        [Authorize]
        public void Logout()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            HttpContext.GetOwinContext().Authentication.SignOut(new AuthenticationProperties
            {
                RedirectUri = Url.Action("Index", "Home")
            },
            
            "Auth0");

        }

        [Authorize]
        public ActionResult UserInfo()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;

            string idToken = claimsIdentity?.Claims.FirstOrDefault(p => p.Type == "id_token").Value;
            string accessToken = claimsIdentity?.Claims.FirstOrDefault(p => p.Type == "access_token").Value;
            string refreshToken = claimsIdentity?.Claims.FirstOrDefault(p => p.Type == "refresh_token").Value;
            string name = claimsIdentity?.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Name)?.Value;
            string emailAddress = claimsIdentity?.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Email)?.Value;
            string profileImg = claimsIdentity?.Claims.FirstOrDefault(p => p.Type == "picture")?.Value;

            return View(new UserInfo(emailAddress, name, profileImg, idToken, accessToken, refreshToken));
        }

        [Authorize(Roles = "admin")]
        public ActionResult Admin()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            string name = claimsIdentity?.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Name)?.Value;
            string emailAddress = claimsIdentity?.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Email)?.Value;
            string profileImg = claimsIdentity?.Claims.FirstOrDefault(p => p.Type == "picture")?.Value;
            return View(new UserInfo(emailAddress, name, profileImg));
        }

        [Authorize]
        public ActionResult Claims()
        {
            return View();
        }
    }
}