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

        // Controller actions here
    }
}
