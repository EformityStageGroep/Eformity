using Microsoft.AspNetCore.Mvc;

namespace Organizer.Services
{
    public class ICurrentTeamService : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
