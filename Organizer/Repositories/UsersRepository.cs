using Microsoft.EntityFrameworkCore;
using Organizer.Contexts;

namespace Organizer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly OrganizerContext _context;

        public UserRepository(OrganizerContext context)
        {
            _context = context;
        }

        public async Task<List<Entities.User>> User()
        {
            return await _context.Users.ToListAsync();
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
