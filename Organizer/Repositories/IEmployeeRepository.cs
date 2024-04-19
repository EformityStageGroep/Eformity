namespace Organizer.Repositories
{
    public interface IEmployeeRepository
    {

        Task Create(Entities.Task task);
        Task Edit(Entities.Task task);
        Task Delete(Guid id);
        Task<int> SaveChangesAsync();
        Task<List<Entities.Task>> GetTasksAsync();
    }
}