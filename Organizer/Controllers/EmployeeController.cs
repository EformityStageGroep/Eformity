using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using Organizer.Contexts;
using Organizer.Entities;
using Organizer.Models;
using Organizer.Repositories;
using Organizer.Services;

namespace Organizer.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _taskRepository;
        private readonly ITeamsRepository _teamRepository;

        public EmployeeController(IEmployeeRepository taskRepository, ITeamsRepository teamRepository)
        {
            _teamRepository = teamRepository;
            _taskRepository = taskRepository;
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
            TeamTaskModel mymodel = new TeamTaskModel();


            List<Entities.Task> tasks = await _taskRepository.GetTasksAsync();
            List<Entities.Team> teams = await _teamRepository.GetTeamsByUser();

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
            if (ModelState.IsValid)
            {
                task.id = Guid.NewGuid();
                await _taskRepository.Create(task);
                await _taskRepository.SaveChangesAsync();
                Console.WriteLine($"Current tenantid controller: {task}");
                return RedirectToAction(nameof(EmployeeDashboard));
            }
            if (!ModelState.IsValid)
            {
                foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Errorrrrr: {modelError.ErrorMessage}");
                }
            }
            Console.WriteLine($"Current tenantid controller: {task}");
            return View(task);
        }
        [HttpPost]

        public async Task<IActionResult> EditTask(Guid id, [Bind("id,title,description,priority,datetime,selectstatus,tenantid,userid")] Entities.Task task)
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
                }
                catch (Exception)
                {
                    // Handle exception, log, etc.
                    throw;
                }
                return RedirectToAction(nameof(EmployeeDashboard));
            }
            if (!ModelState.IsValid)
            {
                foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Errorr: {modelError.ErrorMessage}");
                }
            }
            Console.WriteLine($"Current tenantid EDITtt: {task}");
            return View(task);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _taskRepository.Delete(id);
            await _taskRepository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public  IActionResult EmployeeDashboard2()
        { 
            return View(); 
        }
        }
}
