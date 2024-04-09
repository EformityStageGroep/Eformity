namespace Organizer.Repositories
{
    public interface ITaskRepository
    {
        Task<Organizer.Entities.Task> GetTaskByIdAsync(Guid? id);
    }
}
