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

            TeamTaskModel mymodel = new TeamTaskModel();
       

            List<Entities.Task> tasks = await _taskRepository.GetTaskIdsByUser();
            List<Entities.Team> teams = await _teamRepository.GetTeamsByUser();

            var model = new TeamTaskModel
            {
                Tasks = tasks,
                Teams = teams
            };
            return View(model);

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


                // Get the current user's ID
                var currentUserId = @User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
                var userRole = await _context.Users
                .Where(u => u.id == currentUserId)
                .Select(u => u.Role)
                .FirstOrDefaultAsync();

                bool assignTaskPermission = userRole != null && userRole.assign_task;

                ViewBag.AssignTaskPermission = assignTaskPermission;





            var model = new TeamTaskModel
            {

                Tasks = tasks,
                Teams = teams
            };
           
        return View(model);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,title,description,priority,datetime,selectstatus,tenantid,teamid,userid")] Entities.Task task)
        {
            var userRole = await _context.Users
                .Where(u => u.id == task.userid)
                .Select(u => u.Role)
                .FirstOrDefaultAsync();

            if (userRole != null && userRole.assign_task)
            {
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
            else
            {

                
                task.id = Guid.NewGuid();
                await _taskRepository.Create(task);
                await _taskRepository.SaveChangesAsync();
                
                Console.WriteLine($"Current tenantid controller: {task}");

                return RedirectToAction(nameof(EmployeeDashboard));
            }
            
        }

        /*if (!ModelState.IsValid)
            {
                foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Errorrrrr: {modelError.ErrorMessage}");
                }
            }
            Console.WriteLine($"Current tenantid controller: {task}");
            return View(task);
        }
         */

        [HttpPost]
        public async Task<IActionResult> EditTask(Guid id, [Bind("id,title,description,priority,datetime,selectstatus,tenantid,userid")] Entities.Task task)
        {
            var userRole = await _context.Users
                .Where(u => u.id == task.userid)
                .Select(u => u.Role)
                .FirstOrDefaultAsync();


            if (userRole != null && userRole.assign_task)

            

            {
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
            else
            {
                return RedirectToAction(nameof(EmployeeDashboard));
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
                .Select(u => u.Role)
                .FirstOrDefaultAsync();

            if (userRole != null && userRole.assign_task)
            {
                _context.Task.Remove(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(EmployeeDashboard));
            }
        }


        public IActionResult EmployeeDashboard2()
        {
            return View();
        }
    }
}
