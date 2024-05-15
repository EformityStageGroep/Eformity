using Microsoft.AspNetCore.Mvc;
using Organizer.Repositories;

namespace Organizer.Controllers
{
    public class teamListController : Controller
    {
        private readonly ITeamsRepository _teamRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public teamListController(ITeamsRepository teamRepository, IEmployeeRepository employeeRepository)
        {
            _teamRepository = teamRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<IActionResult> Index()
        {
            var ParentViewModel = await _employeeRepository.ParentViewModel("SideBar");

            await _teamRepository.GetTeamsByUser();
            // Return the view with the model
  
            return View(ParentViewModel);
        }
    }
}
