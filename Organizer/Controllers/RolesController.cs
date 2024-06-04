using Microsoft.AspNetCore.Mvc;
using Organizer.Entities;
using Organizer.Repositories;

namespace Organizer.Controllers
{
    public class RolesController : Controller
    {
        private readonly IRoleRepository _roleRepository;
        private readonly ITeamsRepository _teamRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITasksRepository _tasksRepository;

        public RolesController(IRoleRepository roleRepository, ITeamsRepository teamRepository, IUserRepository userRepository, ITasksRepository tasksRepository)
        {
            _teamRepository = teamRepository;
            _userRepository = userRepository;
            _tasksRepository = tasksRepository;
            _roleRepository = roleRepository;
        }

        // GET: Roles
        public async Task<IActionResult> Index()
        {
            var ParentViewModel = await _tasksRepository.ParentViewModel("Roles");

            return View(ParentViewModel);
        }

        // GET: Roles/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var role = await _roleRepository.GetRoleByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }


        // POST: Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("title,create_team,assign_task,create_task,usermanagement,tenant_id")] Role role)
        {
            if (ModelState.IsValid)
            {
                role.id = Guid.NewGuid();
                await _roleRepository.CreateRoleAsync(role);
                return RedirectToAction(nameof(Index));
            }
            foreach (var state in ModelState)
            {
                var key = state.Key; // Property name
                var errors = state.Value.Errors; // List of errors for the property

                foreach (var error in errors)
                {
                    // Log the error message or handle it as needed
                    Console.WriteLine($"Error in {key}: {error.ErrorMessage}");
                }
            }
            return View(role);
        }

        // POST: Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("id,title,create_team,assign_task,create_task,usermanagement,tenant_id")] Role role)
        {
            if (id != role.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _roleRepository.UpdateRoleAsync(role);
                }
                catch (Exception)
                {
                    if (!RoleExists(role.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));

            }
            if (!ModelState.IsValid)
            {
                foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Errorr: {modelError.ErrorMessage}");
                }
            }
            return View(role);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _roleRepository.DeleteRoleAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool RoleExists(Guid id)
        {
            var role = _roleRepository.GetRoleByIdAsync(id);
            return role != null;
        }
    }
}
