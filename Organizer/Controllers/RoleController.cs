using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Organizer.Entities;
using Organizer.Repositories;

namespace Organizer.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleRepository _roleRepository;

        public RoleController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("tenant_id, title, create_team, assign_task")] Role role)
        {
            if (ModelState.IsValid)
            {
                await _roleRepository.Create(role);
                await _roleRepository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

    }
}
