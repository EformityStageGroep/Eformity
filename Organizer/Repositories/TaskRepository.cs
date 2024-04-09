using Microsoft.EntityFrameworkCore;
using Organizer.Contexts;
using Organizer.Entities;

namespace Organizer.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly OrganizerContext _context;

        public TaskRepository(OrganizerContext context)
        {
            _context = context;
        }

        public async Task<Organizer.Entities.Task> GetTaskByIdAsync(Guid? id)
        {
            return await _context.Tasks.FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
