using Organizer.Repositories;
using Organizer.Entities;

namespace Organizer.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;

        public RoleService(IRoleRepository roleRepository, IUserRepository userRepository)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }

        public async Task<Role> GetRoleForUserAsync(string userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null) throw new Exception("User not found");

            return await _roleRepository.GetRoleByIdAsync(user.role_id);
        }
    }
}
