using Microsoft.AspNetCore.Mvc;
using Organizer.Entities;
using Organizer.Repositories;
using Organizer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Organizer.Controllers
{
    public class RolesController : Controller
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITeamsRepository _teamRepository;



        public RolesController(ITeamsRepository teamRepository, IRoleRepository roleRepository, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _teamRepository = teamRepository;

        }

        // GET: Roles
        public async Task<IActionResult> Index()
        {

            ParentViewModel mymodel = new ParentViewModel();
            List<User> users = await _userRepository.GetUserIdsByTenant();
            List<Role> roles = await _roleRepository.GetAllRolesAsync();
            List<Team> teams = await _teamRepository.GetTeamsByUser();


            // Create the ParentViewModel and populate it with data
            var model = new ParentViewModel
            {
                Users = users,
                Roles = roles,
                Teams = teams
            };

            // Return the view with the model
            return View(model);
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
        public async Task<IActionResult> Create([Bind("title,create_team,assign_task,tenant_id")] Role role)
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
        public async Task<IActionResult> Edit(Guid id, [Bind("id,title,create_team,assign_task.tenant_id")] Role role)
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

        // POST: Roles/AssignRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(Guid userId, Guid role_Id)
        {
            // Retrieve the user and role objects
            var user = await _userRepository.GetUserIdsByTenant();
            var role = await _roleRepository.GetRoleByIdAsync(role_Id);

            if (user == null || role == null)
            {
                // Handle invalid user or role
                return NotFound();
            }

            try
            {
                // Assign the role to the user
                user.role_Id = role_Id;
                await _userRepository.UpdateUserAsync(user);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Handle any errors
                Console.WriteLine($"Error assigning role to user: {ex.Message}");
                return RedirectToAction(nameof(Index)); // Redirect to index page
            }
        }
    }
}
