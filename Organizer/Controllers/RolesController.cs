using Microsoft.AspNetCore.Mvc;
using Organizer.Entities;
using Organizer.Repositories;
using System.Threading.Tasks;

namespace Organizer.Controllers
{
    public class RolesController : Controller
    {
        private readonly IRoleRepository _roleRepository;

        public RolesController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        // GET: Roles
        public async Task<IActionResult> Index()
        {
            var roles = await _roleRepository.GetAllRolesAsync();
            return View(roles);
        }

        // GET: Roles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title, create_team, assign_task")] Role role)
        {
            if (ModelState.IsValid)
            {
                await _roleRepository.CreateRoleAsync(role);
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        // Other action methods for editing, deleting, etc.
    }
}

