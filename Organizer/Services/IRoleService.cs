using Organizer.Entities;

namespace Organizer.Services
{
    public interface IRoleService
    {
        Task<Role> GetRoleForUserAsync(string userId);

    }
}