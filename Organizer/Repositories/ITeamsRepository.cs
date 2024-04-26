namespace Organizer.Repositories
{
    public interface ITeamsRepository
    {
        Task<int> SaveChangesAsync();
        
        Task CreateTeam(Entities.Team team);
        Task CreateUserTeam(Entities.UserTeam userteam);
        Task Delete(string? id);

        Task<List<Entities.Team>> GetTeamsByUser();
        
    }
}
