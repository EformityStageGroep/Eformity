using Organizer.Repositories;
using Organizer.Models;  // Ensure this is where ParentViewModel is defined

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

        public async Task<List<Organizer.Entities.Team>> GetTeamsAsync()
        {
            var teams = await _teamRepository.GetTeamsByUser();
            return teams; // Returns just the list of teams
        }
    }
}