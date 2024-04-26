using Microsoft.EntityFrameworkCore;
using Organizer.Contexts;
using Organizer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Organizer.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly OrganizerContext _context;

        public RoleRepository(OrganizerContext context)
        {
            _context = context;
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async System.Threading.Tasks.Task CreateRoleAsync(Role role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
        }

        // Implement other methods for editing, deleting, etc.
    }
}
