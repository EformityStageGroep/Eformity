using Microsoft.EntityFrameworkCore;
using Organizer.Contexts;
using Organizer.Entities;
using Organizer.Controllers;
using System.Threading.Tasks; // Import this namespace for Task

namespace Organizer.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly OrganizerContext _context;

        public TaskRepository(OrganizerContext context)
        {
            _context = context;
        }

        public async Task<List<Entities.Task>> Task()
        {
            return await _context.Task.ToListAsync();
        }

        public async System.Threading.Tasks.Task Create(Entities.Task task)
        {
            _context.Task.Add(task);
            await _context.SaveChangesAsync(); // Make sure to save changes after adding the task
        }

        public async System.Threading.Tasks.Task Edit(Entities.Task task) {

            _context.Task.Update(task);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task Delete(Guid id)
        {
            var task = await _context.Task.FindAsync(id);
            _context.Task.Remove(task);
            await _context.SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
