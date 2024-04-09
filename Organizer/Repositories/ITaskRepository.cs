using Organizer.Controllers;
namespace Organizer.Repositories
{
    public interface ITaskRepository
    {
        Task<List<Organizer.Entities.Task>> Task();
    }
}
