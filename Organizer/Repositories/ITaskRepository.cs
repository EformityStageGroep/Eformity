namespace Organizer.Repositories
{
    public interface ITaskRepository
    {
        Task<List<Entities.Task>> Task();
        Task Create(Entities.Task task);
        Task Edit(Entities.Task task);
        Task Delete(Guid id);
        Task<List<Entities.Task>> GetTasksByVariable(string Tenant_Id); // Corrected return type
        Task<int> SaveChangesAsync();
    }

}