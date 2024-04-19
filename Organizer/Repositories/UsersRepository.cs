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
            _context.Users.Add(user);
            await _context.SaveChangesAsync(); // Make sure to save changes after adding the task
        }

        public async Task Edit(Entities.User user)
        {

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
