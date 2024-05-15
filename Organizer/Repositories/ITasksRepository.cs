using Organizer.Models;

namespace Organizer.Repositories
{
    public interface ITasksRepository
    {
        Task<List<Entities.Task>> GetTaskIdsByUser();
        Task Create(Entities.Task task);
        Task Edit(Entities.Task task);
        Task Delete(Guid id);
        Task<int> SaveChangesAsync();
        Task<List<Entities.Task>> GetTasksAsync();
        Task<ParentViewModel> ParentViewModel(string pageValue);
    }
}