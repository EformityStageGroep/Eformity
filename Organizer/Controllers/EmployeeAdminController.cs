using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using Organizer.Contexts;
using Organizer.Entities;
using Organizer.Repositories;
using Organizer.Services;

namespace Organizer.Controllers
{
    public class EmployeeAdminController : Controller
    {
        private readonly IEmployeeRepository _taskRepository;

        public EmployeeAdminController(IEmployeeRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }
        // GET: Tasks
        public async Task<IActionResult> Index()
        {
            var tasks = await _taskRepository.GetTasksAsync(); // Fetch tasks based on current tenant
            return View(tasks);
        }
        // GET: Tasks/Details/5

        public async Task<IActionResult> EmployeeAdminDashboard()
        {
            try
            {
                var tasks = await _taskRepository.GetTasksAsync(); // Fetch tasks based on current tenant

                if (tasks == null || !tasks.Any())
                {
                    return NotFound();
                }

                return View(tasks);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as required
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Priority,DateTime,SelectStatus,TenantId")] Entities.Task task)
        {
            if (ModelState.IsValid)
            {
                task.Id = Guid.NewGuid();
                await _taskRepository.Create(task);
                await _taskRepository.SaveChangesAsync();
                return RedirectToAction(nameof(EmployeeAdminDashboard));
            }
            if (!ModelState.IsValid)
            {
                foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Errorrrrr: {modelError.ErrorMessage}");
                }
            }
            Console.WriteLine($"Current TenantId controller: {task}");
            return View(task);
        }
        [HttpPost]

        public async Task<IActionResult> EditTask(Guid id, [Bind("Id,Title,Description,Priority,DateTime,SelectStatus,TenantId")] Entities.Task task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine($"Current TenantId EDIT: {task}");
                    await _taskRepository.Edit(task);
                    await _taskRepository.SaveChangesAsync();
                }
                catch (Exception)
                {
                    // Handle exception, log, etc.
                    throw;
                }
                return RedirectToAction(nameof(EmployeeAdminDashboard));
            }
            if (!ModelState.IsValid)
            {
                foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Errorr: {modelError.ErrorMessage}");
                }
            }
            Console.WriteLine($"Current TenantId EDITtt: {task}");
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

      
    }
}
