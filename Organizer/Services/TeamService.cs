using Organizer.Models;
using Organizer.Repositories;
using Organizer.Services;

namespace Organizer.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamsRepository _teamRepository;
        private readonly IUserRepository _userRepository;

        public TeamService(ITeamsRepository teamRepository, IUserRepository userRepository)
        {
            _teamRepository = teamRepository;
            _userRepository = userRepository;
        }

        public async Task<ParentViewModel> GetParentViewModelAsync()
        {
            var users = await _userRepository.GetUserIdsByTenant();
            var teams = await _teamRepository.GetTeamsByUser();
            var model = new ParentViewModel
            {
                Users = users,
                Teams = teams
            };
            return model;
        }
    }
}