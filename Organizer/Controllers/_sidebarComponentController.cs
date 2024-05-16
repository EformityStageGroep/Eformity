using Organizer.Services; 
using Microsoft.AspNetCore.Mvc;
using Organizer.Repositories;

namespace Organizer.Controllers
{
    public class teamListController : Controller
    {
        private readonly ITeamsRepository _teamRepository;

        public teamListController(ITeamsRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<IActionResult> Index()
        {
            var teams = await _teamRepository.GetTeamsByUser();
            return View(teams);
        }

    }


}
