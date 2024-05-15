
using Organizer.Models;

namespace Organizer.Services
{
    public interface ITeamService
    {
        Task<ParentViewModel> GetParentViewModelAsync();
    }
}
