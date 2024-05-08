using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organizer.Contexts;
using Microsoft.Graph;
using Organizer.Entities;
using Organizer.Models;
using Organizer.Repositories;
using Organizer.Services;

namespace Organizer.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ITeamsRepository _teamRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmployeeRepository _taskRepository;
        private readonly OrganizerContext _context;

        public EmployeeController(ITeamsRepository teamRepository, IUserRepository userRepository, IEmployeeRepository employeeRepository)
        {
            _teamRepository = teamRepository;
            _userRepository = userRepository;
            _taskRepository = employeeRepository;
        }
        // GET: Tasks
        public async Task<IActionResult> Index()
        {
            return View();
        }
        // GET: Tasks/Details/5
        public async Task<IActionResult> EmployeeDashboard()
        {

            ParentViewModel mymodel = new ParentViewModel();
            List<Entities.User> users = await _userRepository.GetUserIdsByTenant();
            List<Entities.Team> teams = await _teamRepository.GetTeamsByUser();
            List<Entities.Task> tasks = await _taskRepository.GetTasksAsync();

            var model = new ParentViewModel
            {
                Users = users,
                Teams = teams,
                Tasks = tasks
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,title,description,priority,datetime,selectstatus,tenantid,teamid,userid")] Entities.Task task)
        {
            var userRole = await _context.Users
                .Where(u => u.id == task.userid)
                .Select(u => u.role_id)
                .FirstOrDefaultAsync();

            if (ModelState.IsValid)
            {
                task.id = Guid.NewGuid();
                await _taskRepository.Create(task);
                await _taskRepository.SaveChangesAsync();
                return RedirectToAction(nameof(EmployeeDashboard));
            }
            else
            {
                return View(task); // Return the same view if ModelState is invalid
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditTask(Guid id, [Bind("id,title,description,priority,datetime,selectstatus,tenantid,userid")] Entities.Task task)
        {
            var userRole = await _context.Users
                .Where(u => u.id == task.userid)
                .Select(u => u.role_id)
                .FirstOrDefaultAsync();

            if (id != task.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine($"Current tenantid EDIT: {task}");
                    await _taskRepository.Edit(task);
                    await _taskRepository.SaveChangesAsync();
                    return RedirectToAction(nameof(EmployeeDashboard));
                }
                catch (Exception)
                {
                    return StatusCode(500, "Internal Server Error");
                }
            }
            else
            {
                return View(task);
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var task = await _context.Task.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            var userRole = await _context.Users
                .Where(u => u.id == task.userid)
                .Select(u => u.role_id)
                .FirstOrDefaultAsync();

            _context.Task.Remove(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult EmployeeDashboard2()
        {
            return View();
        }
    }
}
