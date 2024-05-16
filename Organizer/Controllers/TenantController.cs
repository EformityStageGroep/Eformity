using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Organizer.Controllers
{
    public class TenantController : Controller
    {
        [Authorize(Roles = "SuperAdmin,Tasks")]

        public ActionResult Dashboard()
        {
            if (User.IsInRole("SuperAdmin"))
            {
                return View("ManagerDashboard");
            }
            else if (User.IsInRole("CompanyAdmin"))
            {
                return View("CompanyAdminDashboard");
            }
            else if (User.IsInRole("TasksAdmin"))
            {
                return View("TasksAdminDashboard");
            }
            else if (User.IsInRole("Tasks"))
            {
                return View("TasksDashboard");
            }
            else
            {
                // Handle other roles or unauthorized access
                return RedirectToAction("Unauthorized", "Error");
            }
        }
    }
}