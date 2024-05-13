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

            if (ModelState.IsValid)
            {
                Console.WriteLine($"Task ID: {task.id}, Title: {task.title}, Description: {task.description}, Priority: {task.priority},  TeamId: {task.teamid},etc.");
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
        public async Task<IActionResult> EditTask(Guid id, [Bind("id,title,description,priority,datetime,selectstatus,tenantid,userid,teamid")] Entities.Task task)
        {
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
            await _taskRepository.Delete(id);
            await _taskRepository.SaveChangesAsync();
            return RedirectToAction(nameof(EmployeeDashboard));
        }
        public IActionResult EmployeeDashboard2()
        {
            return View();
        }
    }
}
