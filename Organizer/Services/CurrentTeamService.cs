using Microsoft.AspNetCore.Mvc;
namespace Organizer.Services
{
    public class CurrentTeamService : ICurrentTeamService
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
