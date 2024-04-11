using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Organizer.Controllers
{
    public class RoleController : Controller
    {
        [Authorize(Roles = "SuperAdmin,Employee")]

        public ActionResult Dashboard()
        {
            if (User.IsInRole("SuperAdmin"))
            {
                return View("ManagerDashboard");
            }
            else if (User.IsInRole("Employee"))
            {
                return View("EmployeeDashboard");
            }
            else
            {
                // Handle other roles or unauthorized access
                return RedirectToAction("Unauthorized", "Error");
            }
        }
    }
}
