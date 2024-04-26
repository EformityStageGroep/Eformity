using Organizer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Organizer.Repositories
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetAllRolesAsync();
        System.Threading.Tasks.Task CreateRoleAsync(Role role);
        // Add other methods for editing, deleting, etc.
    }
}
