using Microsoft.EntityFrameworkCore;
using Organizer.Contexts;
using Organizer.Entities;
using Organizer.Services;
using Task = System.Threading.Tasks.Task;

namespace Organizer.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly OrganizerContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICurrentTenantService _currentTenantService;

        public RoleRepository(OrganizerContext context, ICurrentUserService currentUserService, ICurrentTenantService currentTenantService)
        {
            _context = context;
            _currentUserService = currentUserService;
            _currentTenantService = currentTenantService;
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            var tenantId = _currentTenantService.tenantid;
            var roles = await _context.Roles.Where(t => t.tenant_id == tenantId).ToListAsync();
            return roles;
        }

        public async Task<Role> GetRoleByIdAsync(Guid? id)
        {
            return await _context.Roles.FindAsync(id);
        }

        public async Task CreateRoleAsync(Role role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRoleAsync(Role role)
        {
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            var tenantId = _currentTenantService.tenantid; // Corrected variable name to match the query
            return await _context.Roles.AnyAsync(r => r.title == roleName && r.tenant_id == tenantId);
        }

        public async Task DeleteRoleAsync(Guid id)
        {
            // Fetch the default role based on its title
            var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.title == "Default");

            if (defaultRole == null)
            {
                // Handle the case where the default role is not found
                // You can log an error or handle it in some other way
                return;
            }

            var defaultRoleId = defaultRole.id;

            var role = await GetRoleByIdAsync(id);
            if (role != null)
            {
                // Check if the role being deleted is the default role
                bool isDefaultRole = role.id == defaultRoleId;
                Console.WriteLine(isDefaultRole);
                // Remove the role from the database


                // Assign default role to users who had the deleted role
                var usersWithDeletedRole = await _context.Users.Where(u => u.role_id == id).ToListAsync();
                Console.WriteLine($"Amount of things in list: {usersWithDeletedRole.Count}");
                foreach (var user in usersWithDeletedRole)
                {
                    user.role_id = defaultRoleId; // Assign the default role to the user
                    _context.Users.Update(user);
                    Console.WriteLine(user.role_id);
                }
                _context.Roles.Remove(role);
                // Save changes to the database in a single transaction
                await _context.SaveChangesAsync();
            }
        }
    }
}

