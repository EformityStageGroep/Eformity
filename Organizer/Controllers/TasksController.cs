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


namespace Organizer.Controllers
{
    public class TasksController : Controller
    {
        private readonly ITaskRepository _taskRepository;

        public TasksController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }
        // GET: Tasks
        public async Task<IActionResult> Index()
        {
            var tasks = await _taskRepository.Task();
            return View(tasks);
        }
        // GET: Tasks/Details/5
        [HttpPost] // Add this attribute to force POST request
        [ValidateAntiForgeryToken] // Add this attribute for security against CSRF attacks
        public async Task<IActionResult> Details(string Tenant_Id)
        {
            try
            {
                var tasks = await _taskRepository.GetTasksByVariable(Tenant_Id);

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
        public async Task<IActionResult> Create([Bind("Title,Description,Priority,DateTime,Status")] Entities.Task task)
        {
            if (ModelState.IsValid)
            {
                task.Id = Guid.NewGuid();
                await _taskRepository.Create(task);
                await _taskRepository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }
        [HttpPost]

        public async Task<IActionResult> EditTask(Guid id, [Bind("Id,Title,Description,Priority,DateTime")] Entities.Task task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _taskRepository.Edit(task);
                    await _taskRepository.SaveChangesAsync();
                }
                catch (Exception)
                {
                    // Handle exception, log, etc.
                    throw;
                }
                return RedirectToAction(nameof(Details));
            }
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
