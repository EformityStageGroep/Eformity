using Microsoft.AspNetCore.Mvc;
using Organizer.Repositories;

namespace Organizer.Controllers
{
    public class teamListController : Controller
    {
        private readonly ITeamsRepository _teamRepository;
        private readonly ITasksRepository _tasksRepository;

        public teamListController(ITeamsRepository teamRepository, ITasksRepository tasksRepository)
        {
            _teamRepository = teamRepository;
            _tasksRepository = tasksRepository;
        }

        public async Task<IActionResult> Index()
        {
            var ParentViewModel = await _tasksRepository.ParentViewModel("SideBar");

            await _teamRepository.GetTeamsByUser();
            // Return the view with the model
  
            return View(ParentViewModel);
        }
    }
}
