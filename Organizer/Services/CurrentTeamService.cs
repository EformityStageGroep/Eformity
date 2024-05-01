using Microsoft.AspNetCore.Mvc;
using Organizer.Contexts;

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
