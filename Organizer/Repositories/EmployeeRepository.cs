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
            var tenantId = _currentTenantService.tenantid;
            var userid = _currentUserService.userid;
            
            var tasks = await _context.Task.Where(t => t.tenantid == tenantId).ToListAsync();
            var Users = await _context.Task.Where(t => t.userid == userid).ToListAsync();
            // Debugging: Print the fetched tasks

            return Users;
        }

        public async System.Threading.Tasks.Task Create(Entities.Task task)
        {
            task.id = Guid.NewGuid();
            task.tenantid = _currentTenantService.tenantid; // Set tenantid
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