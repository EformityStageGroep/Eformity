
using Organizer.Controllers;
namespace Organizer.Repositories
{
    public interface ITaskRepository
    {
        Task<List<Entities.Task>> Task();
        Task Create(Entities.Task task);
        Task<int> SaveChangesAsync();
        Task Edit(Entities.Task task);
        Task Delete(Guid id);

    }
}