using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Organizer.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        public ActionResult Index()
        {
            // Pass the user profile data to the view
            return View();
        }

        // Sign out action
        public IActionResult SignOut()
        {
            // Perform sign-out
            HttpContext.SignOutAsync();

            // Redirect to home page or any other page after sign-out
            return RedirectToAction("Index", "Home");
        }
    }
}
