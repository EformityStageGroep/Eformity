using Microsoft.AspNetCore.Mvc;

namespace Organizer.Controllers
{
    public class TeamsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
