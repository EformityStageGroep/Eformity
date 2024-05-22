// ITeamService.cs
using System.Threading.Tasks;
using Organizer.Models;

namespace Organizer.Services
{
    public interface ITeamService
    {
        Task<ParentViewModel> GetParentViewModelAsync();
    }
}
