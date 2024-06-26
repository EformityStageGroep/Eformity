﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;

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
            return RedirectToAction("CompanyAdminDashboard", "Users");
        }
    }
}
