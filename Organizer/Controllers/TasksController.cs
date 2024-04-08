using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using Organizer.Contexts;
using Organizer.Entities;

namespace Organizer.Controllers
{
    public class TasksController : Controller
    {
        private readonly OrganizerContext _context;

        public TasksController(OrganizerContext context)
        {
            _context = context;
        }

        // GET: Tasks
        public async Task<IActionResult> Index()
        {
            var tasks = await _context.Task.ToListAsync();
            return View(tasks);
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            try
            { 
                List<Organizer.Entities.Task> task = await _context.Task.ToListAsync();
                if (task == null)
                {
                    return NotFound();
                }
                


                return View(task);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as required
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Task.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        // POST: Tasks/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Priority,DateTime")] Organizer.Entities.Task task)
        {
            if (ModelState.IsValid)
            {
                task.Id = Guid.NewGuid();
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }
        // GET: YourController/EditTask/{id}
        public async Task<IActionResult> EditTask(Guid id)
        {
            var task = await _context.Task.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        // POST: YourController/EditTask/{id}
        [HttpPost]
        
        public async Task<IActionResult> EditTask(Guid id, [Bind("Id,Title,Description,Priority,DateTime")] Organizer.Entities.Task task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    // Handle exception, log, etc.
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
             Console.WriteLine("Deleting task with ID: " + id);
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Task.FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            Console.WriteLine("Deleting task with ID: " + id);
            var task = await _context.Task.FindAsync(id);
            _context.Task.Remove(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskExists(Guid id)
        {
            return _context.Task.Any(e => e.Id == id);
        }
    }
}
