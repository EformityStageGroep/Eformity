using Microsoft.EntityFrameworkCore;
using Organizer.Contexts;
using Organizer.Entities;
using Organizer.Controllers;
using System.Threading.Tasks; // Import this namespace for Task
using Organizer.Services;
using System.Linq;


namespace Organizer.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly OrganizerContext _context;
        private readonly ICurrentTenantService _currentTenantService;

        public TaskRepository(OrganizerContext context, ICurrentTenantService currentTenantService)
        {
            _context = context;
            _currentTenantService = currentTenantService;
        }

        public async Task<List<Entities.Task>> GetTasksAsync() 
        {
            var tenantId = _currentTenantService.TenantId;
            Console.WriteLine($"Current TenantId: {tenantId}"); // Debugging line

            var tasks = await _context.Task.Where(t => t.TenantId == tenantId).ToListAsync();
            // Debugging: Print the fetched tasks
            Console.WriteLine("Fetched Tasks:");
            foreach (var task in tasks)
            {
                Console.WriteLine($"TaskId: {task.Id}, TenantId: {task.TenantId}, Title: {task.Title}, Description: {task.Description}, Priority: {task.Priority}, DateTime: {task.DateTime}, SelectStatus: {task.SelectStatus}");
            }

            return tasks;
        }


        public async System.Threading.Tasks.Task Create(Entities.Task task)
        {
            task.Id = Guid.NewGuid();
            task.TenantId = _currentTenantService.TenantId; // Set TenantId
            var tenantId = _currentTenantService.TenantId;
            Console.WriteLine($"Current TenantIddd: {tenantId}");
            Console.WriteLine($"Current TenantIdddddd: {task.TenantId}");// Debugging line
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
