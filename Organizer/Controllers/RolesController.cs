using Microsoft.AspNetCore.Mvc;
using Organizer.Entities;
using Organizer.Repositories;
using System;
using System.Collections.Generic;
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

        // GET: Roles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("title,create_team,assign_task")] Role role)
        {
            if (ModelState.IsValid)
            {
                role.id = Guid.NewGuid();
                await _roleRepository.CreateRoleAsync(role);
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        // GET: Roles/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var role = await _roleRepository.GetRoleByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        // POST: Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("id,title,create_team,assign_task")] Role role)
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
            return View(role);
        }

        // GET: Roles/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var role = await _roleRepository.GetRoleByIdAsync(id);
            if (role == null)
            {
                return NotFound();
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
