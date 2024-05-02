using Organizer.Services; 
using Microsoft.AspNetCore.Mvc;

namespace Organizer.Controllers
{
    public class teamListController : Controller
    {
        private readonly ITeamService _teamService;

        public teamListController(ITeamService teamService)
        {
            _teamService = teamService;
        }

/*        public async Task<IActionResult> Index()
        {
            var teams = await _teamService.GetTeamsAsync();
            return View(teams);
        }*/

    }


}
