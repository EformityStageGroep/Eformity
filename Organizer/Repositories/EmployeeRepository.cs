using Microsoft.EntityFrameworkCore;
using Organizer.Contexts;
using Organizer.Entities;
using Organizer.Controllers;
using System.Threading.Tasks; // Import this namespace for Task
using Organizer.Services;
using System.Linq;


namespace Organizer.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly OrganizerContext _context;
        private readonly ICurrentTenantService _currentTenantService;
        private readonly ICurrentUserService _currentUserService;

        public EmployeeRepository(OrganizerContext context, ICurrentTenantService currentTenantService, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentTenantService = currentTenantService;
            _currentUserService = currentUserService;
        }

        public async Task<List<Entities.Task>> GetTasksAsync()
        {
            var tenant_id = _currentTenantService.tenant_id;
            var user_id = _currentUserService.user_id;
            
            var tasks = await _context.Task.Where(t => t.tenant_id == tenant_id).ToListAsync();
            var Users = await _context.Task.Where(t => t.user_id == user_id).ToListAsync();
            // Debugging: Print the fetched tasks

            return Users;
        }

        public async System.Threading.Tasks.Task Create(Entities.Task task)
        {
            task.Id = Guid.NewGuid();
            task.tenant_id = _currentTenantService.tenant_id; // Set tenant_id
            _context.Task.Add(task);
            await _context.SaveChangesAsync(); // Make sure to save changes after adding the task
        }

        public async System.Threading.Tasks.Task Edit(Entities.Task task)
        {

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