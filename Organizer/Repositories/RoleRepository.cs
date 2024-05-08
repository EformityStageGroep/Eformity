using Microsoft.EntityFrameworkCore;
using Organizer.Contexts;
using Organizer.Entities;
using Organizer.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<Role> GetRoleByIdAsync(Guid id)
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

        public async Task DeleteRoleAsync(Guid id)
        {
            var role = await GetRoleByIdAsync(id);
            if (role != null)
            {
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
            }
        }
    }
}

