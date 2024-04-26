using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organizer.Contexts;
using Organizer.Services;

namespace Organizer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly OrganizerContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICurrentTenantService _currentTenantService;

        public UserRepository(OrganizerContext context, ICurrentUserService currentUserService, ICurrentTenantService currentTenantService)
        {
            _context = context;
            _currentUserService = currentUserService;
            _currentTenantService = currentTenantService;
        }

        public async Task<List<Entities.User>> User()
        {
            var users = await _context.Users.ToListAsync();
          
            return users;
        }

        public async Task<List<Entities.Team>> GetTeamsByUser()
        {
            var user_id = _currentUserService.user_id;
            // Find the user in the database
            var user = _context.Users
                .Include(u => u.Users_Teams)
                .ThenInclude(ut => ut.Team)
                .FirstOrDefault(u => u.Id == user_id);
            Console.WriteLine("test");
            // Check if the user is found
            if (user == null)
            {
                // Handle the case where the user is not found (return an empty list or handle as needed)
                return new List<Entities.Team>();
            }
            Console.WriteLine("test3");
            // Extract teams from the join table and return them
            var teams = user.Users_Teams.Select(ut => ut.Team).ToList();
            foreach (var task in teams)
            {
                Console.WriteLine($"TaskId: {task.Team_Id}, tenant_id: {task.User_Id}");
            }
            return teams;
        }
        public async Task<List<Entities.User>> GetUserInfo()
        {
            var user_id = _currentUserService.user_id;
            var Users = await _context.Users.Where(t => t.Id == user_id).ToListAsync();
            foreach (var task in Users)
            {
                Console.WriteLine($"user_id: {task.Id}, Email: {task.Email}, Name: {task.FullName}, Tenant: {task.Tenant_Id}");
            }
            return Users;
        }
        public async Task<List<Entities.User>> Getuser_idsByTenant()
        {
            var tenant_id = _currentTenantService.tenant_id;
            // Query to get all user IDs for the specified tenant ID
            var user_ids = await _context.Users
              .Where(user => user.Tenant_Id == tenant_id)
              .ToListAsync();
            foreach (var task in user_ids)
            {
                Console.WriteLine($"TaskId: {task.Id}, tenant_id: {task.Email}, Title: {task.FullName}, Title: {task.Tenant_Id}");
            }
            // Return the list of user IDs
            return user_ids;

        }
        public async Task<List<Entities.User>> GetTasksAsync()
        {
            var user_id = _currentUserService.user_id;

            
            var Users = await _context.Users.ToListAsync();
            // Debugging: Print the fetched tasks
            foreach (var task in Users)
            {
                Console.WriteLine($"TaskId: {task.Id}, tenant_id: {task.Email}, Title: {task.FullName}, Title: {task.Tenant_Id}");
            }
            return Users;
        }
        public async Task Create(Entities.User user)
        {
            // Check if a user with the same ID already exists in the database
            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser != null)
            {
                // A user with the same ID already exists, throw an exception or handle the case appropriately
                return;
            }

            // If the user ID does not exist, add the new user to the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync(); // Make sure to save changes after adding the user
        }

        public async Task Edit(Entities.User user)
        {
            var users = await _context.Users.FindAsync(user);
            _context.Users.Update(users);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(string? id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        public async Task UserExists(string? id)
        {
            _context.Users.Any(e => e.Id == id);
            await _context.SaveChangesAsync();
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
