namespace Organizer.Repositories
{
    public interface IUserRepository
    {
        Task<List<Entities.User>> User();
        Task Create(Entities.User user);
        Task Edit(Entities.User user);
        Task Delete(Guid id);
        Task<int> SaveChangesAsync();
    }
}
