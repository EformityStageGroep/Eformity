using Microsoft.EntityFrameworkCore;
using Organizer.Contexts;
using Organizer.Entities;
using Organizer.Controllers;

namespace Organizer.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly OrganizerContext _context;

        public TaskRepository(OrganizerContext context)
        {
            _context = context;
        }

        public async Task<List<Organizer.Entities.Task>> Task()
        {
            return await _context.Task.FirstOrDefaultAsync();
        }
    }
}
