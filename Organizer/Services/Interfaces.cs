using Organizer.Models;

namespace Organizer.Services.Interfaces
{
    public interface ITeamService
    {
        Task<ParentViewModel> GetParentViewModelAsync();
        Task<List<Organizer.Entities.Team>> GetTeamsAsync();
    }
}
