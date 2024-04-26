namespace Organizer.Repositories
{
    public interface ITeamsRepository
    {
        Task<int> SaveChangesAsync();
        
        Task Create(Entities.Team team);
            
        Task Delete(string? id);

        Task<List<Entities.Team>> GetTeamsByUser();
        
    }
}
