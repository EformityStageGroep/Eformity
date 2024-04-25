namespace Organizer.Repositories
{
    public interface IUserRepository
    {
        Task<List<Entities.User>> User();
        Task Create(Entities.User user);
        Task Edit(Entities.User user);
        Task Delete(string? id);
        Task UserExists(string? id);
        Task<List<Entities.User>> GetUserInfo();
        Task<List<Entities.User>> GetUserIdsByTenant();
        Task<List<Entities.User>> GetTasksAsync();
       
       /* Task<List<Entities.User>> InsertMultipleUsers();*/
        Task<int> SaveChangesAsync();
    }
}
