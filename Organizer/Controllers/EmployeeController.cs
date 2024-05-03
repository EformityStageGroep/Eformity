using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organizer.Contexts;
using Microsoft.Graph;
using Organizer.Entities;
using Organizer.Repositories;
using Organizer.Services;

namespace Organizer.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _taskRepository;
        private readonly OrganizerContext _context;

        public EmployeeController(IEmployeeRepository taskRepository, OrganizerContext context)
        {
            _taskRepository = taskRepository;
            _context = context;
        }
        // GET: Tasks
        public async Task<IActionResult> Index()
        {
            var tasks = await _taskRepository.GetTasksAsync();  // Fetch tasks based on current tenant
            return View(tasks);
        }
        // GET: Tasks/Details/5
        public async Task<IActionResult> EmployeeDashboard()
        {
            try
            {
                var tasks = await _taskRepository.GetTasksAsync(); // Fetch tasks based on current tenant


                if (tasks == null || !tasks.Any())
                {
                    return View(new List<Entities.Task>()); // Return an empty list to the view
                }

                return View(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("title,description,priority,datetime,selectstatus,tenantid,userid")] Entities.Task task)
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
