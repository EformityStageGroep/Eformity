namespace Organizer.Repositories
{
    public interface ITaskRepository
    {
        Task<List<Entities.Task>> Task();
        Task Create(Entities.Task task);
        Task Edit(Entities.Task task);
        Task Delete(Guid id);
        Task<int> SaveChangesAsync();
        Task<List<Entities.Task>> GetTasksAsync();
    }
}